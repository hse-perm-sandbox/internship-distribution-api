using Humanizer;
using InternshipDistribution.Dto;
using InternshipDistribution.Enums;
using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using Microsoft.OpenApi.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace InternshipDistribution.Services
{
    public class ApplicationService
    {
        private readonly ApplicationRepository _applicationRepository;
        private readonly StudentRepository _studentRepository;
        private readonly StudentService _studentService;
        private readonly CompanyRepository _companyRepository;

        public ApplicationService(
            ApplicationRepository repository,
            StudentRepository studentRepository,
            StudentService studentService,
            CompanyRepository companyRepository)
        {
            _applicationRepository = repository;
            _studentRepository = studentRepository;
            _studentService = studentService;
            _companyRepository = companyRepository; 
        }

        public async Task<DistributionApplicationDto> CreateApplicationAsync(ApplicationInput dto)
        {
            await _studentService.CheckAccess(dto.StudentId);

            var application = new DistributionApplication
            {
                StudentId = dto.StudentId,
                Priority1CompanyId = dto.Priority1CompanyId,
                Priority2CompanyId = dto.Priority2CompanyId,
                Priority3CompanyId = dto.Priority3CompanyId,
                Status = ApplicationStatus.Created
            };

            await ValidatePriorities(application);

            for (int i = 1; i<=3; i++)
                UpdatePriorityStatus(application, i);

            await _applicationRepository.AddAsync(application);

            var applicationDto = new DistributionApplicationDto();

            UpdateDtoFromModel(application, applicationDto);

            return applicationDto;
        }

        public async Task<IEnumerable<DistributionApplicationDto>> GetAllApplicationsAsync()
        {
            var applications = await _applicationRepository.GetAllAsync();

            var applicationsDto = new List<DistributionApplicationDto>();

            foreach (var application in applications)
            {
                var dto = new DistributionApplicationDto();

                UpdateDtoFromModel(application, dto);

                applicationsDto.Add(dto);
            }

            return applicationsDto;
        }

        public async Task<DistributionApplicationDto> UpdatePrioritiesAsync(int id, UpdatePrioritiesInput dto)
        {
            var application = await _applicationRepository.GetByIdAsync(id);
            if (application == null)
                throw new KeyNotFoundException($"Заявка с ID {id} не найдена");

            UpdateApplicationFromDto(dto, application);

            await ValidatePriorities(application);

            for (int i = 1; i <= 3; i++)
                UpdatePriorityStatus(application, i);

            await _applicationRepository.UpdateAsync(application);

            var applicationDto = new DistributionApplicationDto();

            UpdateDtoFromModel(application, applicationDto);

            return applicationDto;
        }

        public async Task<DistributionApplicationDto?> GetByStudentIdAsync(int studentId)
        {
            var application = await _applicationRepository.GetByStudentIdAsync(studentId);

            if (application == null)
                throw new KeyNotFoundException($"Заявка на распределение с id студента = {studentId} не найдена");

            var applicationDto = new DistributionApplicationDto();

            UpdateDtoFromModel(application, applicationDto);

            return applicationDto;
        }

        public async Task<DistributionApplicationDto?> GetApplicationAsync(int id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);

            if (application == null)
                throw new KeyNotFoundException($"Заявка на распределение с id = {id} не найдена");

            var applicationDto = new DistributionApplicationDto();

            UpdateDtoFromModel(application, applicationDto);

            return applicationDto;
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);

            if (application == null)
                throw new KeyNotFoundException($"Заявка на распределение с id = {id} не найдена");

            return await _applicationRepository.SoftDeleteAsync(id);
        }

        private async Task ValidatePriorities(DistributionApplication application)
        {
            var companyIds = new[]
            {
            application.Priority1CompanyId,
            application.Priority2CompanyId,
            application.Priority3CompanyId
            }.Where(id => id.HasValue).Cast<int>().ToList();

            foreach (var companyId in companyIds)
            {
                if (!await _companyRepository.ExistsAsync(companyId))
                    throw new KeyNotFoundException($"Компания с id = {companyId} не найдена");
            }

            if (companyIds.Distinct().Count() != companyIds.Count)
                throw new ValidationException("Компании в приоритетах не должны повторяться");
        }

        public void UpdateDtoFromModel(DistributionApplication application, DistributionApplicationDto applicationDto)
        {
            applicationDto.Id = application.Id;
            applicationDto.StudentId = application.StudentId;
            applicationDto.Priority1CompanyId = application.Priority1CompanyId;
            applicationDto.Priority2CompanyId = application.Priority2CompanyId;
            applicationDto.Priority3CompanyId = application.Priority3CompanyId;
            applicationDto.Status = application.Status.Humanize();
            applicationDto.Priority1Status = application.Priority1Status.Humanize();
            applicationDto.Priority2Status = application.Priority2Status.Humanize();
            applicationDto.Priority3Status = application.Priority3Status.Humanize();
        }

        private void UpdateApplicationFromDto(UpdatePrioritiesInput dto, DistributionApplication application)
        {
            if (dto.Priority1CompanyId.HasValue)
                application.Priority1CompanyId = dto.Priority1CompanyId;

            if (dto.Priority2CompanyId.HasValue)
                application.Priority2CompanyId = dto.Priority2CompanyId;

            if (dto.Priority3CompanyId.HasValue)
                application.Priority3CompanyId = dto.Priority3CompanyId;
        }

        private void UpdatePriorityStatus(DistributionApplication application, int priorityNumber)
        {
            switch (priorityNumber)
            {
                case 1 when application.Priority1CompanyId != null:
                    application.Priority1Status = PriorityStatus.Sent;
                    break;
                case 2 when application.Priority2CompanyId != null:
                    application.Priority2Status = PriorityStatus.Sent;
                    break;
                case 3 when application.Priority3CompanyId != null:
                    application.Priority3Status = PriorityStatus.Sent;
                    break;
            }

            application.Status = DetermineApplicationStatus(application);
        }

        private ApplicationStatus DetermineApplicationStatus(DistributionApplication application)
        {
            return application.Priority1Status == PriorityStatus.Sent ||
                   application.Priority2Status == PriorityStatus.Sent ||
                   application.Priority3Status == PriorityStatus.Sent
                ? ApplicationStatus.UnderReview
                : ApplicationStatus.Created;
        }
    }
}

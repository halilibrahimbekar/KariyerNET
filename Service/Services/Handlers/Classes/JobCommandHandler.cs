using Common.Common.Enum;
using Common.Common.Helper;
using Data.Common.Enum;
using KariyerNET.Data;
using MediatR;
using Service.Entities;
using Service.Services.Commands.Request;
using Service.Services.Commands.Response;
using Service.Services.Handlers.Interfaces;
using Service.Services.Queries.Request;
using Service.Services.Search;
using Service.Services.Search.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services.Handlers
{
    public class AddJobCommandHandler : IHandler, IRequestHandler<AddJobRequest, AddJobResponse>
    {
        private readonly KariyerNETContext _context;
        private readonly ElasticSearchManager _elasticSearchManager;

        public AddJobCommandHandler(KariyerNETContext context, ElasticSearchManager elasticSearchManager)
        {
            _context = context;
            _elasticSearchManager = elasticSearchManager;
        }

        /// <summary>
        /// Yeni job ekler
        /// </summary>
        /// <param name="request">yeni job propertylerini içeren obje</param>
        /// <param name="cancellationToken">the cancellation token</param>
        /// <returns></returns>
        public async Task<AddJobResponse> Handle(AddJobRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new AddJobResponse { Id = Guid.Empty, Status = ResultStatusEnum.UnSuccess };

                if (!_context.Companies.Any(x => x.Id == request.CompanyId))
                {
                    response.Message = "Şirket / İşveren Bulunamadı.";
                    return response;
                }
                else if (_context.Companies.First(x => x.Id == request.CompanyId).RemainingAdvertCount <= 0)
                {
                    response.Message = "İlan Hakkınız Bulunmuyor.";
                    return response;
                }

                var id = Guid.NewGuid();
                var job = new Job
                {
                    Id = id,
                    Position = request.Position,
                    Description = request.Description,
                    Salary = request.Salary,
                    Additional = request.Additional,
                    EndDate = DateTime.Now.AddDays(Convert.ToInt32(Settings.GetValue(SettingsEnum.DefaultAddvertCount.ToString()))), // TODO avoid hardcoded variables
                    Score = CalculateScore(request),
                    CompanyId = request.CompanyId,
                    JobType = (int)request.JobType,
                    CreatedDateTime = DateTime.Now
                };

                _context.Jobs.Add(job);

                var comnapny = _context.Companies.FirstOrDefault(x => x.Id == request.CompanyId);
                comnapny.RemainingAdvertCount--;

                _context.SaveChanges();

                _ = Task.Run(() => IndexJobAdvertToElastic(job));

                return new AddJobResponse { Id = id, Status = ResultStatusEnum.Success };
            }
            catch (Exception ex)
            {
                // TODO : error handling...

                return new AddJobResponse { Status = ResultStatusEnum.Error, Message = ex.Message };
            }
        }

        /// <summary>
        /// İlan Skorunu hesaplar
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private int CalculateScore(AddJobRequest request)
        {
            var score = 0;
            if (request.Salary.HasValue)
            {
                score++;
            }

            if (request.JobType.HasValue)
            {
                score++;
            }

            if (!string.IsNullOrEmpty(request.Additional))
            {
                score++;
            }

            var restrictedWordString = Settings.GetValue(SettingsEnum.RetrictedWords.ToString());

            if (!string.IsNullOrEmpty(restrictedWordString))
            {
                var restrictedWords = restrictedWordString.Split(',');
                if (!restrictedWords.All(x => request.Description.Contains(x)))
                {
                    score += 2;
                }
            }

            return score;
        }

        /// <summary>
        /// Eklenen ilanı elastic'e indexler
        /// </summary>
        /// <param name="job"></param>
        private void IndexJobAdvertToElastic(Job job)
        {
            try
            {
                _elasticSearchManager.Index(
                        new JobIndex
                        {
                            Description = job.Description,
                            EndDate = job.EndDate,
                            JobType = (JobType?)job.JobType,
                            Position = job.Position,
                            Salary = job.Salary,
                            Score = job.Score,
                            CompanyId = job.CompanyId
                        });
            }
            catch
            {
                // TODO Error handling.
                throw;
            }
        }
    }

    public class GetJobCommandHandler : IHandler, IRequestHandler<GetJobByIdRequest, GetJobResponse>
    {
        private readonly KariyerNETContext _context;

        public GetJobCommandHandler(KariyerNETContext context)
        {
            _context = context;
        }

        public async Task<GetJobResponse> Handle(GetJobByIdRequest request, CancellationToken cancellationToken)
        {
            var job = _context.Jobs.FirstOrDefault(x => x.Id == request.Id);
            if (job != null)
            {
                return new GetJobResponse
                {
                    Description = job.Description,
                    EndDate = job.EndDate,
                    JobType = (JobType?)job.JobType,
                    Position = job.Position,
                    Salary = job.Salary,
                    Score = job.Score
                };
            }
            else return null;
        }
    }

    public class GetJobListCommandHandler : IHandler, IRequestHandler<GetJobListRequest, List<GetJobResponse>>
    {
        private readonly ElasticSearchManager _elasticSearchManager;

        public GetJobListCommandHandler(ElasticSearchManager elasticSearchManager)
        {
            _elasticSearchManager = elasticSearchManager;
        }

        public async Task<List<GetJobResponse>> Handle(GetJobListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var searchResult = _elasticSearchManager.SearchJob(request).Result;

                var result = searchResult.Documents.Select(x =>
                        new GetJobResponse
                        {
                            Description = x.Description,
                            EndDate = x.EndDate,
                            JobType = (JobType?)x.JobType,
                            Position = x.Position,
                            Salary = x.Salary,
                            Score = x.Score
                        }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                // TODO : Error handling..
                throw;
            }
        }
    }

    public class CreateIndexCommandHandler : IHandler, IRequestHandler<CreateIndexRequest, CreateIndexResponse>
    {
        private readonly ElasticSearchManager _elasticSearchManager;

        public CreateIndexCommandHandler(ElasticSearchManager elasticSearchManager)
        {
            _elasticSearchManager = elasticSearchManager;
        }

        public async Task<CreateIndexResponse> Handle(CreateIndexRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var searchResult = _elasticSearchManager.CreateIndex();
                return new CreateIndexResponse { Message = searchResult ? "Başarılı!" : "Başarısız'" };
            }
            catch (Exception ex)
            {
                // TODO : Error handling..
                throw;
            }
        }
    }
}

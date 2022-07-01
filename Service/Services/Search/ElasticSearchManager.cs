using Data.Common.Enum;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using Service.Services.Queries.Request;
using Service.Services.Search.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services.Search
{
    public class ElasticSearchManager
    {
        private string ServerUrls { get; set; }

        private string IndexName => "kariyernet";

        public ElasticSearchManager(IConfiguration configuration)
        {
            this.ServerUrls = configuration.GetConnectionString("ElasticSearch");
        }

        public ElasticSearchManager(string serverUrl)
        {
            this.ServerUrls = serverUrl;
        }

        public ElasticClient ElasticClient
        {
            get
            {
                return CreateElasticClient(this.ServerUrls, this.IndexName);
            }
        }

        private ElasticClient CreateElasticClient(string serverUrl, string indexName)
        {
            var settings = new ConnectionSettings(new Uri(serverUrl));
            settings.DefaultIndex(indexName);
            settings.DefaultMappingFor<JobIndex>(x => x.IndexName(indexName));
            settings.EnableDebugMode(); //If you want show ES query
            settings.ServerCertificateValidationCallback((o, certificate, chain, errors) => true);
            settings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);
            var client = new ElasticClient(settings);

            return client;
        }

        public bool CreateIndex()
        {
            var indexSettings = new IndexSettings
            {
                NumberOfReplicas = 1,
                NumberOfShards = 1
            };

            var createIndexDescriptor = new CreateIndexDescriptor(this.IndexName)
            .Map<JobIndex>(x => x.AutoMap())
            .InitializeUsing(new IndexState() { Settings = indexSettings });

            var response = this.ElasticClient.Indices.Create(createIndexDescriptor);
            var result = this.ElasticClient.Indices.Exists(this.IndexName).Exists || response.Acknowledged;

            return result;
        }

        public bool Bulk(BulkDescriptor descriptor)
        {
            return this.ElasticClient.Bulk(descriptor).IsValid;
        }

        public bool Index<T>(T data) where T : class
        {
            var indexResponse = this.ElasticClient.Index(data, i => i.Index(this.IndexName));
            return indexResponse.IsValid;
        }

        public bool IndexMany<T>(IEnumerable<T> data) where T : class
        {
            var indexResponse = this.ElasticClient.IndexMany(data);
            return indexResponse.IsValid;
        }

        public bool Delete<T>(int id) where T : class
        {
            var result = this.ElasticClient.Delete<T>(long.Parse(id.ToString()), descriptor => descriptor.Index(this.IndexName));
            return result.Result == Result.Deleted;
        }

        public long GetAllCount<T>() where T : class
        {
            var count = this.ElasticClient.Count<T>();
            return count.Count;
        }

        public List<T> GetAllData<T>(int pageIndex, int pageSize) where T : class
        {
            var sortDescriptor = new Func<SortDescriptor<T>, IPromise<IList<ISort>>>(s => s.Descending("enddate"));
            var list = this.ElasticClient.Search<T>(x => x.From(pageIndex).Take(pageSize).MatchAll().Sort(sortDescriptor));
            return list.Documents.ToList();
        }

        public T GetById<T>(int id) where T : class
        {
            var result = ElasticClient.Get(new DocumentPath<T>(id), descriptor => descriptor.Index(this.IndexName));
            return result.Source;
        }

        public async Task<JobSearchResponse> SearchJob(GetJobListRequest filter)
        {
            var searchRequest = new SearchRequest();
            var filters = new List<QueryContainer>();
            PreparePostFilters(filters, filter);
            var boolQuery = new BoolQuery { Filter = filters };
            var startPage = (filter.PageNumber - 1) * filter.PageSize;
            var result = await this.ElasticClient.SearchAsync<JobIndex>(x => x.From(startPage)
                                .Take(filter.PageSize)
                                .TrackTotalHits(true)
                                .PostFilter(x => boolQuery)
                                .Sort(GetSort(filter.Sort)));

            return new JobSearchResponse { Count = result.Total, Documents = result.Documents };
        }

        private void PreparePostFilters(List<QueryContainer> filters, GetJobListRequest filter)
        {
            if (filter.MinEndDate.HasValue || filter.MaxEndDate.HasValue)
            {
                var searchFilter = new QueryContainer(new DateRangeQuery { Field = "enddate", GreaterThanOrEqualTo = filter.MinEndDate ?? DateTime.MinValue, LessThanOrEqualTo = filter.MaxEndDate ?? DateTime.MaxValue });
                filters.Add(searchFilter);
            }

            if (filter.CompanyId != Guid.Empty)
            {
                var searchFilter = new QueryContainer(new BoolQuery { Must = new List<QueryContainer> { new MatchQuery { Field = new Field("companyId"), Query = filter.CompanyId.ToString() } } });
                filters.Add(searchFilter);
            }
        }

        private Func<SortDescriptor<JobIndex>, IPromise<IList<ISort>>> GetSort(SearchJobSortEnum? sortType)
        {
            return sortType switch
            {
                SearchJobSortEnum.EndDateAsc => new Func<SortDescriptor<JobIndex>, IPromise<IList<ISort>>>(s => s.Ascending("endDate")),
                SearchJobSortEnum.EndDateDesc => new Func<SortDescriptor<JobIndex>, IPromise<IList<ISort>>>(s => s.Descending("endDate")),
                _ => new Func<SortDescriptor<JobIndex>, IPromise<IList<ISort>>>(s => s.Descending("endDate")),
            };
        }
    }
}

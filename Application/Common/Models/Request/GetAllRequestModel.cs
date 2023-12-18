namespace Application.Common.Models.Request
{
    public class GetAllRequestModel
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public GetAllRequestModel(int skip, int take)
        {
            this.Skip = skip;
            this.Take = take;
        }

        public GetAllRequestModel()
        {
                
        }
    }
}
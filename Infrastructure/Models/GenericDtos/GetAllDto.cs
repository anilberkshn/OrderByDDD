namespace Infrastructure.Models.GenericDtos
{
    public class GetAllDto
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public GetAllDto(int skip, int take)
        {
            this.Skip = skip;
            this.Take = take;
        }

        public GetAllDto()
        {
                
        }
    }
}
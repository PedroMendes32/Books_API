namespace Books.API.Response
{
    public class BookResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public List<string> Authors { get; set; }
        public BookResponse()
        {
                
        }
    }
}

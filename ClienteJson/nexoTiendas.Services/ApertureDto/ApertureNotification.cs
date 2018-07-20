namespace nexoTiendas.ApertureDto
{
    public class ApertureNotification
    {
       public ApertureNotification(string topic, string resource)
        {
            Topic = topic;
            Resource = resource;
        }
        public string Topic { get; set; }
        public string Resource { get; set; }
    }
}

namespace MusicPlayer.RESTful
{
    public class Container<T>
    {
        public T ObjetoContainer { get; set; }
        public Link[] Links { get; set; }
    }
}
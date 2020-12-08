using System.Collections.Generic;

namespace MusicPlayer.RESTful
{
    public class Hateoas
    {
        private string _url;
        private string _protocolo = "https://";
        public List<Link> actions = new List<Link>();

        public Hateoas(string url)
        {
            _url = url;
        }

        public Hateoas(string url, string protocolo)
        {
            _url = url;
            _protocolo = protocolo;
        }

        public void AddAction(string rel, string method)
        {
            actions.Add(new Link(_protocolo  + _url, rel, method));
        }

        public Link[] GetActions(string sufixo)
        {
            Link[] links =  new Link[actions.Count];

            for (int i = 0; i < links.Length; i++)
            {
                links[i] = new Link(actions[1].Href, actions[i].Rel, actions[i].Method);
            }

            foreach (var link in links)
            {
                if (!link.Method.Contains("PATCH"))
                {
                    link.Href += "/" + sufixo;
                }
            }

            return links;
        }
    }
}
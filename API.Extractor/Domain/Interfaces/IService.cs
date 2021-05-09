using System;
using System.Threading.Tasks;

namespace API.Extractor.Domain.Interfaces
{
    public interface IService
    {
        public string Name { get; }
        public void Configure();
    }
}

using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Pusula.Training.HealthCare.Titles
{
    public class TitleManager(ITitleRepository titleRepository) : DomainService
    {
        public virtual async Task<Title> CreateAsync(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), TitleConsts.NameMaxLength, TitleConsts.NameMinLength);

            var title = new Title(
                Guid.NewGuid(),
                name
            );

            return await titleRepository.InsertAsync(title);
        }

        public virtual async Task<Title> UpdateAsync(Guid id, string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), TitleConsts.NameMaxLength, TitleConsts.NameMinLength);

            var title = await titleRepository.GetAsync(id);
            title.Name = name;

            return await titleRepository.UpdateAsync(title);
        }

        public virtual async void DeleteAsync(Guid id)
        {
            var title = await titleRepository.GetAsync(id);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(title?.Name, nameof(title));
            await titleRepository.DeleteAsync(id);
        }
    }
}

using AutoMapper;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetItemResponse>> GetItemsAsync()
        {
            var item = _itemRepository.GetAll();
            var response = _mapper.Map<IEnumerable<GetItemResponse>>(item);

            return response;
        }
    }
}

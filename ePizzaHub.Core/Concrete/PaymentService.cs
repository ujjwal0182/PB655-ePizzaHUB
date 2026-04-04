using AutoMapper;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository,IOrderRepository orderRepository,IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<string> MakePayment(MakePaymentRequest paymentRequest)
        {
            var paymentModel = _mapper.Map<PaymentDetail>(paymentRequest);

            if (paymentRequest.OrderRequest is not null
                    && paymentRequest.OrderRequest.OrderItems.Count > 0)
            {
                var orderDetails = MapOrderDetails(paymentRequest, paymentModel);

                await _paymentRepository.AddAsync(paymentModel);

                await _orderRepository.AddNewOrder(orderDetails);

                int rowsAffected = await _paymentRepository.CommitAsync();

                return rowsAffected.ToString();
            }
            return string.Empty; //if user is not passing the correct value
        }

        // This method maps the MakePaymentRequest to Order details, including setting the PaymentId and UserId from the PaymentDetail model.
        private Order MapOrderDetails(MakePaymentRequest paymentRequest, PaymentDetail paymentModel)
        {
            var orderDetails = _mapper.Map<Order>(paymentRequest.OrderRequest);

            orderDetails.PaymentId = paymentModel.Id;
            orderDetails.UserId = paymentModel.UserId;
            orderDetails.CreatedDate = paymentModel.CreatedDate;

            orderDetails.OrderItems.ToList().ForEach(x => x.OrderId = orderDetails.Id);

            return orderDetails;
        }
    }
}

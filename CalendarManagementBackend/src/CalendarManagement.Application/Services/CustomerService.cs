using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Exceptions;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Services
{
    public class CustomerService(ICustomerRepository repository) : ICustomerService
    {
        public async Task<List<CustomerDto>> GetAllAsync(CancellationToken ct = default)
        {
            var customers = await repository.GetAllAsync(ct);
            return customers.Select(ToDto).ToList();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var customer = await repository.GetByIdAsync(id, ct);
            return customer is null ? null : ToDto(customer);
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
        {
            Validate(request.Name, request.ContactPerson, request.Email, request.Phone);

            var customer = new Customer
            {
                Name = request.Name.Trim(),
                ContactPerson = request.ContactPerson?.Trim(),
                Email = request.Email?.Trim(),
                Phone = request.Phone?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await repository.AddAsync(customer, ct);
            return ToDto(customer);
        }

        public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerRequest request, CancellationToken ct = default)
        {
            Validate(request.Name, request.ContactPerson, request.Email, request.Phone);

            var customer = await repository.GetByIdAsync(id, ct);
            if (customer is null)
            {
                return null;
            }

            customer.Name = request.Name.Trim();
            customer.ContactPerson = request.ContactPerson?.Trim();
            customer.Email = request.Email?.Trim();
            customer.Phone = request.Phone?.Trim();
            // CreatedAt ändras inte – den sattes när kunden skapades

            await repository.UpdateAsync(customer, ct);
            return ToDto(customer);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            repository.DeleteAsync(id, ct);

        private static void Validate(string? name, string? contactPerson, string? email, string? phone)
        {
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors["name"] = ["Namn måste fyllas i."];
            }

            CheckMaxLength(errors, "name", name, Customer.NameMaxLength, "Namn");
            CheckMaxLength(errors, "contactPerson", contactPerson, Customer.ContactPersonMaxLength, "Kontaktperson");
            CheckMaxLength(errors, "email", email, Customer.EmailMaxLength, "E-post");
            CheckMaxLength(errors, "phone", phone, Customer.PhoneMaxLength, "Telefon");

            if (errors.Count > 0)
            {
                throw new RequestValidationException(errors);
            }
        }

        private static void CheckMaxLength(
            Dictionary<string, string[]> errors, string field, string? value, int maxLength, string label)
        {
            if (value is not null && value.Trim().Length > maxLength)
            {
                errors[field] = [$"{label} får vara högst {maxLength} tecken."];
            }
        }

        private static CustomerDto ToDto(Customer c) => 
            new(c.Id, c.Name, c.ContactPerson, c.Email, c.Phone);
    }
}
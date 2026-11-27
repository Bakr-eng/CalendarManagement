using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Exceptions;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Application.Validation;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Domain.Enums;

namespace CalendarManagement.Application.Services
{
    public class CustomerService(ICustomerRepository repository) : ICustomerService
    {
        public async Task<List<CustomerDto>> GetAllAsync(CancellationToken ct = default)
        {
            var customers = await repository.GetAllAsync(ct);

            // I listan döljs de sista fyra siffrorna i personnummer
            return customers.Select(c => ToDto(c, maskIdentityNumber: true)).ToList();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var customer = await repository.GetByIdAsync(id, ct);

            // I detaljerna visas hela numret
            return customer is null ? null : ToDto(customer);
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
        {
            var identityNumber = Validate(
                request.Name, request.ContactPerson, request.Email, request.Phone,
                request.CustomerType, request.IdentityNumber);

            var customer = new Customer
            {
                Name = request.Name.Trim(),
                CustomerType = request.CustomerType,
                IdentityNumber = identityNumber,
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
            var identityNumber = Validate(
                request.Name, request.ContactPerson, request.Email, request.Phone,
                request.CustomerType, request.IdentityNumber);

            var customer = await repository.GetByIdAsync(id, ct);
            if (customer is null)
            {
                return null;
            }

            customer.Name = request.Name.Trim();
            customer.CustomerType = request.CustomerType;
            customer.IdentityNumber = identityNumber;
            customer.ContactPerson = request.ContactPerson?.Trim();
            customer.Email = request.Email?.Trim();
            customer.Phone = request.Phone?.Trim();
            // CreatedAt ändras inte – den sattes när kunden skapades

            await repository.UpdateAsync(customer, ct);
            return ToDto(customer);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            repository.DeleteAsync(id, ct);

        // Kastar RequestValidationException om något är fel.
        // Returnerar identitetsnumret i rätt format (eller null om det inte fylldes i).
        private static string? Validate(
            string? name, string? contactPerson, string? email, string? phone,
            CustomerType customerType, string? identityNumber)
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

            if (!Enum.IsDefined(customerType))
            {
                errors["customerType"] = ["Kundtyp måste vara Company eller Private."];
            }

            string? normalizedNumber = null;

            if (!string.IsNullOrWhiteSpace(identityNumber))
            {
                if (customerType == CustomerType.Private)
                {
                    normalizedNumber = IdentityNumberValidator.NormalizePersonalNumber(identityNumber);
                    if (normalizedNumber is null)
                    {
                        // Numret skrivs inte ut i felmeddelandet – det är en känslig uppgift
                        errors["identityNumber"] = ["Ogiltigt personnummer. Skriv ÅÅÅÅMMDD-XXXX."];
                    }
                }
                else if (customerType == CustomerType.Company)
                {
                    normalizedNumber = IdentityNumberValidator.NormalizeOrganisationNumber(identityNumber);
                    if (normalizedNumber is null)
                    {
                        errors["identityNumber"] = ["Ogiltigt organisationsnummer. Skriv NNNNNN-NNNN."];
                    }
                }
            }

            if (errors.Count > 0)
            {
                throw new RequestValidationException(errors);
            }

            return normalizedNumber;
        }

        private static void CheckMaxLength(
            Dictionary<string, string[]> errors, string field, string? value, int maxLength, string label)
        {
            if (value is not null && value.Trim().Length > maxLength)
            {
                errors[field] = [$"{label} får vara högst {maxLength} tecken."];
            }
        }

        private static CustomerDto ToDto(Customer c, bool maskIdentityNumber = false)
        {
            var identityNumber = c.IdentityNumber;

            // Bara personnummer maskeras. Organisationsnummer är offentliga.
            if (maskIdentityNumber && c.CustomerType == CustomerType.Private && identityNumber is not null)
            {
                identityNumber = IdentityNumberValidator.MaskPersonalNumber(identityNumber);
            }

            return new(c.Id, c.Name, c.ContactPerson, c.Email, c.Phone, c.CustomerType, identityNumber);
        }
    }
}
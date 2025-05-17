using SP.Application.Service.Interface;
using SP.Domain.Entity;
using SP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Service.Implement
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task CreateEmployee(Employee employee)
        {
            _unitOfWork.EmployeeRepository.AddAsync(employee);
            return _unitOfWork.SaveChangeAsync();

            
        }

        public async Task DeleteEmployee(Guid id)
        {
            var result = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (result != null)
            {
                await _unitOfWork.EmployeeRepository.DeleteAsync(result);
                await _unitOfWork.SaveChangeAsync();
            }
           
                    
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _unitOfWork.EmployeeRepository.GetAllAsync();
            
        }

        public async Task<Employee> GetEmployeeById(Guid id)
        {
            return await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var result = await _unitOfWork.EmployeeRepository.GetByIdAsync(employee.Id);
            if (result != null)
            {
                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.SaveChangeAsync();
            }
            
        }
    }
}

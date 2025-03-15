using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using spr311_web_api.BLL.Dtos.Role;
using spr311_web_api.DAL.Entities.Identity;

namespace spr311_web_api.BLL.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateAsync(CreateRoleDto dto)
        {
            var entity = new AppRole
            {
                Name = dto.Name
            };

            var result = await _roleManager.CreateAsync(entity);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            var result = await _roleManager.DeleteAsync(entity);
            return result.Succeeded;
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
         {
            var entities = await _roleManager.Roles.ToListAsync();

            var dtos = entities.Select(e => new RoleDto
            {
                Id = e.Id,
                Name = e.Name ?? "Noname"
            });

            return dtos;
        }

        public async Task<RoleDto?> GetByIdAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if(entity == null)
            {
                return null;
            }

            var dto = new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name ?? "noname"
            };

            return dto;
        }

        public async Task<bool> UpdateAsync(UpdateRoleDto dto)
        {
            var entity = new AppRole
            {
                Id = dto.Id,
                Name = dto.Name
            };

            var result = await _roleManager.UpdateAsync(entity);
            return result.Succeeded;
        }
    }
}

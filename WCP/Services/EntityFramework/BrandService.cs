using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.DTOs;
using WCPShared.Models.Entities;
using WCPShared.Models.Views;
using WCPShared.Services.Converters;

namespace WCPShared.Services.EntityFramework
{
    public class BrandService : GenericEFService<Brand>, IDtoExtensions<BrandDto, Brand>, IObjectViewService<Brand, BrandView>
    {
        private readonly IWcpDbContext _context;
        private readonly OrganizationService _organizationService;
        private readonly ViewConverter _viewConverter;

        public BrandService(IWcpDbContext context, OrganizationService organizationService, ViewConverter viewConverter) : base(context)
        {
            _context = context;
            _organizationService = organizationService;
            _viewConverter = viewConverter;
        }

        public async Task<Brand?> UpdateObject(int id, BrandDto brand)
        {
            Brand? oldBrand = await GetObject(id);

            if (oldBrand is null)
                return null!;

            oldBrand.Name = brand.Name;
            oldBrand.URL = brand.URL;

            if (oldBrand.OrganizationId != brand.OrganizationId)
            {
                var org = await _organizationService.GetObject(brand.OrganizationId);

                if (org is not null)
                {
                    oldBrand.OrganizationId = brand.OrganizationId;
                    oldBrand.Organization = org;
                }
            }

            _context.Update(oldBrand);
            await _context.SaveChangesAsync();
            return oldBrand;
        }

        public async Task<Brand?> AddObject(BrandDto obj)
        {
            Organization? organization = await _organizationService.GetObject(obj.OrganizationId);
            if (organization is null)
                return null;

            _context.Organizations.Attach(organization);
            Brand brand = new Brand
            {
                Name = obj.Name,
                OrganizationId = obj.OrganizationId,
                Organization = organization,
                URL = obj.URL,
            };

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<List<BrandView>> GetObjectsViewBy(Expression<Func<Brand, bool>> predicate)
        {
            return await _context.Brands
                .Where(predicate)
                .Include(x => x.Organization)
                .ThenInclude(x => x.Language)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<BrandView?> GetObjectViewBy(Expression<Func<Brand, bool>> predicate)
        {
            var brand = await _context.Brands
                .Include(x => x.Organization)
                .ThenInclude(x => x.Language)
                .SingleOrDefaultAsync(predicate);

            if (brand is not null)
                return _viewConverter.Convert(brand);
            return null;
        }

        public async Task<List<BrandView>> GetAllObjectsView()
        {
            return await _context.Brands
                .Include(x => x.Organization)
                .ThenInclude(x => x.Language)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }
    }
}

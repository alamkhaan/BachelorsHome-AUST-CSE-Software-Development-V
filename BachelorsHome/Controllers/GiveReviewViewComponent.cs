using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorsHome.Data;
using BachelorsHome.Models;
using Microsoft.AspNetCore.Mvc;
namespace BachelorsHome.Controllers
{
    public class GiveReviewViewComponent : ViewComponent
    {
        private readonly BachelorsHomeProductContext _context;
        private readonly BachelorsHomeContext _context2;
        public GiveReviewViewComponent(BachelorsHomeProductContext context, BachelorsHomeContext context2)
        {
            _context = context;
            _context2 = context2;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            return await Task.FromResult((IViewComponentResult)View("GiveReview"));
        }
    }
}

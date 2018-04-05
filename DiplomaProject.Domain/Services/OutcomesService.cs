using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Services
{
    public class OutcomesService
    {
        private IDPService dataService;
        public OutcomesService(IDPService dataService)
        {
            this.dataService = dataService;
        }

        public async Task SaveDependencies(List<Edge> model)
        {
            var professionId = model.First()?.ProfessionId;
            var dbDependencies = dataService.GetAll<Edge>().Where(e => e.ProfessionId == professionId).Select(e => e.Id);
            var deletedDependencies = dbDependencies.Except(model.Select(e => e.Id));

            foreach (var edge in deletedDependencies)
            {
                await dataService.DeleteById<Edge>(edge);
            }
            var inserted = model.Where(e => e.Id <= 0);
            var updated = model.Where(e => e.Id > 0);

            foreach (var edge in inserted)
            {
                await dataService.Insert<Edge>(edge);
            }

            foreach (var edge in updated)
            {
                await dataService.Update<Edge>(edge);
            }
        }
    }
}

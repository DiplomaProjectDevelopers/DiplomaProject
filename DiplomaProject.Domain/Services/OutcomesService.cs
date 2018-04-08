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

        public async Task<IEnumerable<Edge>> SaveDependencies(List<Edge> model)
        {
            var professionId = model.First()?.ProfessionId;
            var dbDependencies = dataService.GetAll<Edge>().Where(e => e.ProfessionId == professionId).Select(e => e.Id).ToList();
            var deletedDependencies = dbDependencies.Except(model.Select(e => e.Id)).ToList();

            foreach (var edge in deletedDependencies)
            {
                await dataService.DeleteById<Edge>(edge);
            }
            var inserted = model.Where(e => e.Id <= 0).ToList();
            var updated = model.Where(e => e.Id > 0).ToList();

            for (int i = 0; i < inserted.Count; ++i)
            {
                inserted[i].Id = 0;
                inserted[i] = await dataService.Insert<Edge>(inserted[i]);
            }

            for (int i = 0; i < updated.Count; ++i)
            {
               updated[i] = await dataService.Update<Edge>(updated[i]);
            }
            return model;
        }
    }
}

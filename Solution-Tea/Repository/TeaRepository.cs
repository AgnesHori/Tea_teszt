using Microsoft.EntityFrameworkCore;
using Solution_Tea.Context;
using Solution_Tea.Modells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Tea.Repository
{
    public class TeaRepository: BaseRepository <Tea>
    {
        public override ObservableCollection<Tea> GetAll()
        {
            using (AppDbContext _context = new AppDbContext())
            {
                List<Tea> items = _context.Set<Tea>()
                                          .AsNoTracking()
                                          .Include(x => x.Type)
                                          .ToList();

                return new ObservableCollection<Tea>(items);
            }
        }
    }
}

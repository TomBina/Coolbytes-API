using System;
using System.Collections.Generic;
using System.Linq;
using CoolBytes.Core.Models;

namespace CoolBytes.Core.Collections
{
    public class ExperienceCollection : UpdatableCollection<Experience>
    {
        public override void Update(IEnumerable<Experience> items)
        {
            var itemsToRemove = Items.Except(items, Experience.IdComparer).ToArray();
            RemoveRange(itemsToRemove);

            foreach (var experience in items)
            {
                if (experience.Id == 0)
                {
                    Items.Add(experience);
                }
                else
                {
                    var match = Items.FirstOrDefault(i => i.Id == experience.Id);
                    if (match != null)
                    {
                        match.Update(experience.Name, experience.Color, experience.Image);
                    }
                    else
                    {
                        Items.Add(experience);
                    }
                }
            }        
        }

        public override bool Exists(Experience item) =>
            Items.Any(e => e.Id == item.Id);
    }
}
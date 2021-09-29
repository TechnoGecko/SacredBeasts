using System.Collections.Generic;
using System.Text;
using PlatformerGameKit;
using UnityEngine;

namespace Combat
{
    public partial struct Hit
    {


        public Transform source;

        public Team team;

        public int damage;

        public Hit(Transform source, Team team, int damage, HashSet<ITarget> ignore = null)
        {
            target = null;
            this.source = source;
            this.team = team;
            this.damage = damage;
            this.ignore = ignore;
        }

        partial void AppendDetails(StringBuilder text)
        {
            text.Append($", {nameof(source)}='").Append(source != null ? source.name : "null")
                .Append($"', {nameof(team)}=").Append(team)
                .Append($", {nameof(damage)}=").Append(damage);
        }
    }
}
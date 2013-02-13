using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Model.Entities.Interfaces;

namespace Model.Entities
{
    public class Unit:MapItem
    {
        public float WeaponRange { get; set; }
        public float Damage { get; set; }
        public float SightRange { get; set; }

        public PointF TargetLocation;
        public Unit CurrentEnemy;

        public float MaxSpeed { get; set; }


        public Unit()
        {
            WeaponRange = 0.4F;
            Damage = 3;
            HitPoints = 200;
            TargetLocation = new PointF();
            MaxSpeed = 0.03F;
        }
        

        public override void Update()
        {
            this.Location.X += Velocity.X;
            this.Location.Y += Velocity.Y;
            if (Attacking)
            {
                CurrentEnemy.HitPoints -= (int)this.Damage;
                if (CurrentEnemy.HitPoints <= 0)
                {
                    CurrentEnemy = null;
                    Attacking = false;
                }
            }
            foreach (Unit item in GetUnitsInCollusionRange())
            {
                item.GetOutofMyWayBitch(this);
            }
        }
        bool Attacking = false;
        public void Act()
        {
            Attacking = false;
            var EnemyUnitsInSightRange = GetEnemyUnitsInSightRange();

            if (EnemyUnitsInSightRange.Count == 0)
            {
                this.Velocity = this.Owner.DefaultVelocity;
                this.TargetLocation = this.Owner.TargetLocation;
            }
            else
            {
                var closestEnemy = EnemyUnitsInSightRange.OrderBy(a => Distance(a)).Where(a=>a.HitPoints>0).FirstOrDefault();
                if (Distance(closestEnemy) <= this.WeaponRange)
                {
                    CurrentEnemy = closestEnemy;
                    this.Velocity = new PointF();
                    Attacking = true;
                }
                else
                {
                    TargetLocation = closestEnemy.Location;
                }
            }
            if (!Attacking)
            {
                double angle = Math.Atan2(TargetLocation.Y - this.Location.Y, TargetLocation.X - this.Location.X);
                Velocity = new PointF((float)Math.Cos(angle) * MaxSpeed, (float)Math.Sin(angle) * MaxSpeed);

            }
            if (Velocity.X != 0 && Velocity.Y != 0)
            {
                SoftCollisionAvoidance(); 
            }
        }
        public void SoftCollisionAvoidance()
        {
            CollisionRangeList.Clear();
            foreach (Unit OtherUnit in MyArena.AllUnits.Where(a=>a!=this && Distance(a)<0.2))
            {
                AngleRange CollisionRange = CalculateCollisionRange(OtherUnit);
                CollisionRangeList.Add(CollisionRange);
            }
            List<PointF> SolutionSet = new List<PointF>();
            for (int d = 0; d <= 360; d+=5)
            {
                for (float r = 0; r <= MaxSpeed; r+=MaxSpeed/10)
                {
                    PointF tmp = Add(this.Location, new PointF((float)Math.Cos(DegreeToRadian(d)) * r, (float)Math.Sin(DegreeToRadian(d)) * r));
                    bool IsValid = true;
                    foreach (AngleRange CollisionRange in CollisionRangeList)
                    {
                        if (CollisionRange.IsInRange(tmp))
                        {
                            IsValid = false;
                        }
                    }
                    if (IsValid)
                    {
                        SolutionSet.Add(tmp);
                    }
                }
            }
            PointF NewPoint=SolutionSet.OrderBy(a => Arenas.Arena.GetDistance(a, TargetLocation)).FirstOrDefault();
            if (!NewPoint.IsEmpty)
            {
                Velocity = new PointF(NewPoint.X - Location.X, NewPoint.Y - Location.Y);
            }
            else
            {
                Velocity = new PointF();
            }
        }
        private double DegreeToRadian(int deg)
        {
            return Math.PI * deg / 180;
        }
        private AngleRange CalculateCollisionRange(Unit OtherUnit)
        {
            float d = Distance(OtherUnit);
            double RSum = this.Radius + OtherUnit.Radius;
            double ConeAngle = Math.Asin(OtherUnit.Radius / d);

            float deltaX = OtherUnit.Location.X - this.Location.X;
            float deltaY = OtherUnit.Location.Y - this.Location.Y;

            double CollisionAngle = Math.Atan2(deltaY , deltaX);
            AngleRange CollisionRange = new AngleRange();

            CollisionRange.First = CollisionAngle + ConeAngle;
            CollisionRange.Last = CollisionAngle - ConeAngle;

            CollisionRange.Apex = Add(this.Location, new PointF((this.Velocity.X + OtherUnit.Velocity.X) / 2, (this.Velocity.Y + OtherUnit.Velocity.Y) / 2));

            return CollisionRange;
        }
        public void GetOutofMyWayBitch(Unit u)
        {
            float d=Distance(u);
            float rSum = (float)(u.Radius + this.Radius);
            float push = (rSum - d) / 2;
            double angle = Math.Atan2(u.Location.Y - this.Location.Y, u.Location.X - this.Location.X);

            float pushX = (float)Math.Cos(angle) * push;
            float pushY = (float)Math.Sin(angle) * push;

            this.Location.X -= pushX;
            this.Location.Y -= pushY;
            u.Location.X += pushX;
            u.Location.Y += pushY;
        }
        private List<Unit> GetUnitsInCollusionRange()
        {
            var ret = MyArena.AllUnits.Where(x => this.Distance(x) < this.Radius + x.Radius).ToList();
            return ret;
        }

        public override void Paint(Graphics g)
        {
            base.Paint(g);
            if (Attacking)
            {
                Pen Pen1 = new Pen(Color.Yellow);
                PointF EnemyScreenCoordinate=ToScreenCoordinate(CurrentEnemy.Location);
                g.DrawLine(Pen1, this.ScreenCoordinate, EnemyScreenCoordinate);
                g.DrawRectangle(Pen1, EnemyScreenCoordinate.X - 2, EnemyScreenCoordinate.Y - 2, 4, 4);
            }
        }

        private List<Unit> GetEnemyUnitsInSightRange()
        {
            return MyArena.AllUnits.Where(x => x.Owner.Up != this.Owner.Up && Arenas.Arena.GetDistance(this.Location, x.Location) <= this.SightRange * MyArena.TILE_SIZE_X ).ToList();
        }

        private float Distance(Unit to)
        {
            return Arenas.Arena.GetDistance(this.Location, to.Location);
        }

    }
}

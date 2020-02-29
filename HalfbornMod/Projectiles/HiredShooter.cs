using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace HalfbornMod.Projectiles
{
	
	public abstract class HiredShooter : Minion
	{
		
		public virtual void CreateDust()
		{
		}

		
		public virtual void SelectFrame()
		{
		}

		
		public override void Behavior()
		{
			Player player = Main.player[base.projectile.owner];
			float num = (float)base.projectile.width * this.spacingMult;
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (i != base.projectile.whoAmI && projectile.active && projectile.owner == base.projectile.owner && projectile.type == base.projectile.type && Math.Abs(base.projectile.position.X - projectile.position.X) + Math.Abs(base.projectile.position.Y - projectile.position.Y) < num)
				{
					if (base.projectile.position.X < Main.projectile[i].position.X)
					{
						Projectile projectile2 = base.projectile;
						projectile2.velocity.X = projectile2.velocity.X - this.idleAccel;
					}
					else
					{
						Projectile projectile3 = base.projectile;
						projectile3.velocity.X = projectile3.velocity.X + this.idleAccel;
					}
					if (base.projectile.position.Y < Main.projectile[i].position.Y)
					{
						Projectile projectile4 = base.projectile;
						projectile4.velocity.Y = projectile4.velocity.Y - this.idleAccel;
					}
					else
					{
						Projectile projectile5 = base.projectile;
						projectile5.velocity.Y = projectile5.velocity.Y + this.idleAccel;
					}
				}
			}
			Vector2 value = base.projectile.position;
			float num2 = this.viewDist;
			bool flag = false;
			base.projectile.tileCollide = true;
			for (int j = 0; j < 200; j++)
			{
				NPC npc = Main.npc[j];
				if (npc.CanBeChasedBy(this, false))
				{
					float num3 = Vector2.Distance(npc.Center, base.projectile.Center);
					if ((num3 < num2 || !flag) && Collision.CanHitLine(base.projectile.position, base.projectile.width, base.projectile.height, npc.position, npc.width, npc.height))
					{
						num2 = num3;
						value = npc.Center;
						flag = true;
					}
				}
			}
			if (Vector2.Distance(player.Center, base.projectile.Center) > (flag ? 1000f : 500f))
			{
				base.projectile.ai[0] = 1f;
				base.projectile.netUpdate = true;
			}
			if (base.projectile.ai[0] == 1f)
			{
				base.projectile.tileCollide = false;
			}
			if (flag && base.projectile.ai[0] == 0f)
			{
				Vector2 value2 = value - base.projectile.Center;
				if (value2.Length() > this.chaseDist)
				{
					value2.Normalize();
					base.projectile.velocity = (base.projectile.velocity * this.inertia + value2 * this.chaseAccel) / (this.inertia + 1f);
				}
				else
				{
					base.projectile.velocity *= (float)Math.Pow(0.97, 40.0 / (double)this.inertia);
				}
			}
			else
			{
				if (!Collision.CanHitLine(base.projectile.Center, 1, 1, player.Center, 1, 1))
				{
					base.projectile.ai[0] = 1f;
				}
				float num4 = 6f;
				if (base.projectile.ai[0] == 1f)
				{
					num4 = 15f;
				}
				Vector2 center = base.projectile.Center;
				Vector2 vector = player.Center - center;
				base.projectile.netUpdate = true;
				int num5 = 1;
				for (int k = 0; k < base.projectile.whoAmI; k++)
				{
					if (Main.projectile[k].active && Main.projectile[k].owner == base.projectile.owner && Main.projectile[k].type == base.projectile.type)
					{
						num5++;
					}
				}
				vector.X -= (float)((10 + num5 * 40) * player.direction);
				vector.Y -= 70f;
				float num6 = vector.Length();
				if (num6 > 200f && num4 < 9f)
				{
					num4 = 9f;
				}
				if (num6 < 100f && base.projectile.ai[0] == 1f && !Collision.SolidCollision(base.projectile.position, base.projectile.width, base.projectile.height))
				{
					base.projectile.ai[0] = 0f;
					base.projectile.netUpdate = true;
				}
				if (num6 > 2000f)
				{
					base.projectile.Center = player.Center;
				}
				if (num6 > 48f)
				{
					vector.Normalize();
					vector *= num4;
					float num7 = this.inertia / 2f;
					base.projectile.velocity = (base.projectile.velocity * num7 + vector) / (num7 + 1f);
				}
				else
				{
					base.projectile.direction = Main.player[base.projectile.owner].direction;
					base.projectile.velocity *= (float)Math.Pow(0.9, 40.0 / (double)this.inertia);
				}
			}
			base.projectile.rotation = base.projectile.velocity.X * 0.05f;
			this.SelectFrame();
			this.CreateDust();
			if (base.projectile.velocity.X > 0f)
			{
				base.projectile.spriteDirection = (base.projectile.direction = -1);
			}
			else if (base.projectile.velocity.X < 0f)
			{
				base.projectile.spriteDirection = (base.projectile.direction = 1);
			}
			base.projectile.ai[1] += 1f;
			if (base.projectile.ai[0] == 0f && flag)
			{
				if ((value - base.projectile.Center).X > 0f)
				{
					base.projectile.spriteDirection = (base.projectile.direction = -1);
				}
				else if ((value - base.projectile.Center).X < 0f)
				{
					base.projectile.spriteDirection = (base.projectile.direction = 1);
				}
				if (base.projectile.ai[1] >= 0f && Main.myPlayer == base.projectile.owner)
				{
					Vector2 vector2 = value - base.projectile.Center;
					if (vector2 == Vector2.Zero)
					{
						vector2 = new Vector2(0f, 1f);
					}
					vector2.Normalize();
					vector2 *= this.shootSpeed;
					int num8 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, vector2.X, vector2.Y, this.shoot, (int)(40f), base.projectile.knockBack, Main.myPlayer, 0f, 0f);
					Main.projectile[num8].timeLeft = 300;
					Main.projectile[num8].netUpdate = true;
					base.projectile.netUpdate = true;
					base.projectile.ai[1] = (float)(-(float)this.shootCool);
				}
			}
		}

		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = true;
			return true;
		}

		
		protected float idleAccel = 0.05f;

		
		protected float spacingMult = 1f;

		
		protected float viewDist = 100f;

		
		protected float chaseDist = 250f;

		
		protected float chaseAccel = 5f;

		
		protected float inertia = 40f;

		
		protected int shootCool = 45;

		
		protected float shootSpeed;

		
		protected int shoot;
	}
}

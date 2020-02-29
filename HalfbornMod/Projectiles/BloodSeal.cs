
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HalfbornMod.Projectiles
{
    public class BloodSeal : ModProjectile
    {
        NPC target;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Seal");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public bool IsStickingToTarget
        {
            get => projectile.ai[0] == 1f;
            set => projectile.ai[0] = value ? 1f : 0f;
        }


        public int TargetWhoAmI
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        private const int MAX_STICKY_JAVELINS = 6; // This is the max. amount of javelins being able to attach
        private readonly Point[] _stickingJavelins = new Point[MAX_STICKY_JAVELINS]; // The point array holding for sticking javelins

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            IsStickingToTarget = true; // we are sticking to a target
            TargetWhoAmI = target.whoAmI; // Set the target whoAmI
            projectile.velocity =
                (target.Center - projectile.Center) *
                0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            projectile.netUpdate = true; // netUpdate this javelin

            projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore
            target.AddBuff(ModContent.BuffType<Buffs.CritStrike>(), 900);
            // It is recommended to split your code into separate methods to keep code clean and clear
            // UpdateStickyJavelins(target);
        }

        /*
		 * The following code handles the javelin sticking to the enemy hit.
		 */
        /* private void UpdateStickyJavelins(NPC target)
         {
             int currentJavelinIndex = 0; // The javelin index

             for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
             {
                 Projectile currentProjectile = Main.projectile[i];
                 if (i != projectile.whoAmI // Make sure the looped projectile is not the current javelin
                     && currentProjectile.active // Make sure the projectile is active
                     && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                     && currentProjectile.type == projectile.type // Make sure the projectile is of the same type as this javelin
                     && currentProjectile.modProjectile is ExampleJavelinProjectile javelinProjectile // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
                     && javelinProjectile.IsStickingToTarget // the previous pattern match allows us to use our properties
                     && javelinProjectile.TargetWhoAmI == target.whoAmI)
                 {

                     _stickingJavelins[currentJavelinIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                     if (currentJavelinIndex >= _stickingJavelins.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
                         break;
                 }
             }

             // Remove the oldest sticky javelin if we exceeded the maximum
             if (currentJavelinIndex >= MAX_STICKY_JAVELINS)
             {
                 int oldJavelinIndex = 0;
                 // Loop our point array
                 for (int i = 1; i < MAX_STICKY_JAVELINS; i++)
                 {
                     // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                     if (_stickingJavelins[i].Y < _stickingJavelins[oldJavelinIndex].Y)
                     {
                         oldJavelinIndex = i; // Remember the index of the removed javelin
                     }
                 }
                 // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                 Main.projectile[_stickingJavelins[oldJavelinIndex].X].Kill();
             }
         }*/

        // Added these 2 constant to showcase how you could make AI code cleaner by doing this
        // Change this number if you want to alter how long the javelin can travel at a constant speed
        private const int MAX_TICKS = 45;

        // Change this number if you want to alter how the alpha changes
        private const int ALPHA_REDUCTION = 25;

        public override void AI()
        {
            // Run either the Sticky AI or Normal AI
            // Separating into different methods helps keeps your AI clean
            if (IsStickingToTarget) StickyAI();
            NormalAI();
        }

        private void NormalAI()
        {
           
            //TargetWhoAmI++;

            // For a little while, the javelin will travel with the same speed, but after this, the javelin drops velocity very quickly.
            if (TargetWhoAmI >= MAX_TICKS)
            {
                //TargetWhoAmI = MAX_TICKS; // set ai1 to maxTicks continuously
            }
            for (int i = 0; i < 200; i++)
            {
                 if (Main.npc[i].life > Main.npc[i + 1].life)
                 {
                     target = Main.npc[i];
                 }
                 else
                 {
                     target = Main.npc[i + 1];
                 }
                target = Main.npc[i];

                {

                    float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                    float shootToY = target.position.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));


                    if (distance < 480f && !target.friendly && target.active)
                    {

                        distance = 3f / distance;


                        shootToX *= distance * 5;
                        shootToY *= distance * 5;


                        projectile.velocity.X = shootToX;
                        projectile.velocity.Y = shootToY;
                    }
                }
            }
        }

        private void StickyAI()
        {
            projectile.damage = 0;
            int projTargetIndex = (int)TargetWhoAmI;
            const int aiFactor = 90; // Change this factor to change the 'lifetime' of this sticking javelin
            projectile.localAI[0] += 1f;

            // Every 30 ticks, the javelin will perform a hit effect

            if (projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
            { // If the index is past its limits, kill it
                projectile.Kill();
            }
            else if (Main.npc[projTargetIndex].active)
            { // If the target is active and can take damage
              // Set the projectile's position relative to the target's center
              //projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
              //projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;                
            }
            else
            { // Otherwise, kill the projectile
                projectile.Kill();
            }
        }
    }
}
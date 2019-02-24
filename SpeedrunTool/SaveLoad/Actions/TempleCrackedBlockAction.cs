using System.Collections.Generic;
using Celeste.Mod.SpeedrunTool.SaveLoad.Component;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.SpeedrunTool.SaveLoad.Actions {
    public class TempleCrackedBlockAction : AbstractEntityAction {
        private Dictionary<EntityID, TempleCrackedBlock> saveEntities = new Dictionary<EntityID, TempleCrackedBlock>();

        public override void OnQuickSave(Level level) {
            saveEntities = level.Tracker.GetDictionary<TempleCrackedBlock>();
        }

        private void RestoreTempleCrackedBlockPosition(
            On.Celeste.TempleCrackedBlock.orig_ctor_EntityID_EntityData_Vector2 orig, TempleCrackedBlock self,
            EntityID eid, EntityData data, Vector2 offset) {
            EntityID entityId = data.ToEntityId();
            self.SetEntityId(entityId);
            orig(self, eid, data, offset);

            if (IsLoadStart) {
                if (saveEntities.ContainsKey(entityId))
                    self.Position = saveEntities[entityId].Position;
                else
                    self.Add(new RemoveSelfComponent());
            }
        }

        public override void OnClear() {
            saveEntities.Clear();
        }

        public override void OnLoad() {
            On.Celeste.TempleCrackedBlock.ctor_EntityID_EntityData_Vector2 += RestoreTempleCrackedBlockPosition;
        }

        public override void OnUnload() {
            On.Celeste.TempleCrackedBlock.ctor_EntityID_EntityData_Vector2 -= RestoreTempleCrackedBlockPosition;
        }

        public override void OnInit() {
            typeof(TempleCrackedBlock).AddToTracker();
        }
    }
}
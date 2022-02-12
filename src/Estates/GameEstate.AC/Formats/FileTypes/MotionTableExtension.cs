//using GameEstate.AC.Formats.Entity;
//using GameEstate.AC.Formats.Props;
//using GameEstate.Explorer;
//using GameEstate.Formats;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace GameEstate.AC.Formats.FileTypes
//{
//    [PakFileType(PakFileType.MotionTable)]
//    public static class MotionTableExtension
//    {
//        //public float GetCycleLength(MotionStance stance, MotionCommand motion)
//        //{
//        //    var key = (uint)stance << 16 | (uint)motion & 0xFFFFF;
//        //    if (!Cycles.TryGetValue(key, out var motionData) || motionData == null) return 0.0f;
//        //    var length = 0.0f;
//        //    foreach (var anim in motionData.Anims) length += GetAnimationLength(anim);
//        //    return length;
//        //}

//        //public List<float> GetAttackFrames(uint motionTableId, MotionStance stance, MotionCommand motion, MotionCommand? currentMotion = null)
//        //{
//        //    var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(motionTableId);

//        //    var animData = GetAnimData(stance, motion, currentMotion ?? GetDefaultMotion(stance));

//        //    var frameNums = new List<int>();
//        //    var totalFrames = 0;

//        //    foreach (var anim in animData)
//        //    {
//        //        var animation = DatManager.PortalDat.ReadFromDat<Animation>(anim.AnimId);
//        //        foreach (var frame in animation.PartFrames)
//        //        {
//        //            foreach (var hook in frame.Hooks) if (hook.HookType == AnimationHookType.Attack) frameNums.Add(totalFrames);
//        //            totalFrames++;
//        //        }
//        //    }
//        //    var attackFrames = new List<float>();
//        //    foreach (var frameNum in frameNums) attackFrames.Add((float)frameNum / totalFrames);    // div 0?

//        //    // cache?
//        //    return attackFrames;
//        //}

//        //public float GetAnimationLength(MotionStance stance, MotionCommand motion, MotionCommand currentMotion)
//        //{
//        //    var animData = GetAnimData(stance, motion, currentMotion);
//        //    var length = 0.0f;
//        //    foreach (var anim in animData) length += GetAnimationLength(anim);
//        //    return length;
//        //}

//        //public float GetAnimationLength(AnimData anim)
//        //{
//        //    var highFrame = anim.HighFrame;
//        //    // get the maximum # of animation frames
//        //    var animation = DatManager.PortalDat.ReadFromDat<Animation>(anim.AnimId);
//        //    if (anim.HighFrame == -1) highFrame = (int)animation.NumFrames;
//        //    if (highFrame > animation.NumFrames)
//        //    {
//        //        // magic windup for level 6 spells appears to be the only animation w/ bugged data
//        //        //Console.WriteLine($"MotionTable.GetAnimationLength({anim}): highFrame({highFrame}) > animation.NumFrames({animation.NumFrames})");
//        //        highFrame = (int)animation.NumFrames;
//        //    }
//        //    var numFrames = highFrame - anim.LowFrame;
//        //    return numFrames / Math.Abs(anim.Framerate); // framerates can be negative, which tells the client to play in reverse
//        //}

//        //public ACE.Entity.Position GetAnimationFinalPositionFromStart(ACE.Entity.Position position, float objScale, MotionCommand motion)
//        //{
//        //    var defaultStyle = (MotionStance)DefaultStyle;
//        //    var defaultMotion = GetDefaultMotion(defaultStyle); // get the default motion for the default
//        //    return GetAnimationFinalPositionFromStart(position, objScale, defaultMotion, defaultStyle, motion);
//        //}

//        //public ACE.Entity.Position GetAnimationFinalPositionFromStart(ACE.Entity.Position position, float objScale, MotionCommand currentMotionState, MotionStance style, MotionCommand motion)
//        //{
//        //    var length = 0F; // init our length var...will return as 0 if not found
//        //    var finalPosition = new ACE.Entity.Position();
//        //    var motionHash = ((uint)currentMotionState & 0xFFFFFF) | ((uint)style << 16);

//        //    if (Links.ContainsKey(motionHash))
//        //    {
//        //        var links = Links[motionHash];
//        //        if (links.ContainsKey((uint)motion))
//        //        {
//        //            // loop through all that animations to get our total count
//        //            for (var i = 0; i < links[(uint)motion].Anims.Count; i++)
//        //            {
//        //                var anim = links[(uint)motion].Anims[i];
//        //                uint numFrames;
//        //                // check if the animation is set to play the whole thing, in which case we need to get the numbers of frames in the raw animation
//        //                if ((anim.LowFrame == 0) && (anim.HighFrame == -1))
//        //                {
//        //                    var animation = DatManager.PortalDat.ReadFromDat<Animation>(anim.AnimId);
//        //                    numFrames = animation.NumFrames;
//        //                    if (animation.PosFrames.Count > 0)
//        //                    {
//        //                        finalPosition = position;
//        //                        var origin = new Vector3(position.PositionX, position.PositionY, position.PositionZ);
//        //                        var orientation = new Quaternion(position.RotationX, position.RotationY, position.RotationZ, position.RotationW);
//        //                        foreach (var posFrame in animation.PosFrames)
//        //                        {
//        //                            origin += Vector3.Transform(posFrame.Origin, orientation) * objScale;

//        //                            orientation *= posFrame.Orientation;
//        //                            orientation = Quaternion.Normalize(orientation);
//        //                        }

//        //                        finalPosition.PositionX = origin.X;
//        //                        finalPosition.PositionY = origin.Y;
//        //                        finalPosition.PositionZ = origin.Z;

//        //                        finalPosition.RotationW = orientation.W;
//        //                        finalPosition.RotationX = orientation.X;
//        //                        finalPosition.RotationY = orientation.Y;
//        //                        finalPosition.RotationZ = orientation.Z;
//        //                    }
//        //                    else return position;
//        //                }
//        //                else numFrames = (uint)(anim.HighFrame - anim.LowFrame);

//        //                length += numFrames / Math.Abs(anim.Framerate); // Framerates can be negative, which tells the client to play in reverse
//        //            }
//        //        }
//        //    }

//        //    return finalPosition;
//        //}
//    }
//}

﻿using System;
using System.Collections.Generic;
using GL_EditorFramework.GL_Core;
using GL_EditorFramework.Interfaces;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;

namespace FirstPlugin.Turbo.CourseMuuntStructs
{
    public class RenderableConnectedPaths : AbstractGlDrawable
    {
        private static ShaderProgram defaultShaderProgram;

        public List<PathGroup> PathGroups = new List<PathGroup>();

        public void AddGroup(PathGroup group)
        {
            PathGroups.Add(new PathGroup()
            {
                PathPoints = group.PathPoints,
            });
        }

        public override void Prepare(GL_ControlLegacy control)
        {
        }

        public override void Prepare(GL_ControlModern control)
        {
            var defaultFrag = new FragmentShader(
      @"#version 330
				void main(){
					gl_FragColor = vec4(0,1,0,1);
				}");

            var defaultVert = new VertexShader(
              @"#version 330
				in vec4 position;
			
                uniform mat4 mtxCam;
                uniform mat4 mtxMdl;

				void main(){
					gl_Position = mtxCam  * mtxMdl * vec4(position.xyz, 1);
				}");

            defaultShaderProgram = new ShaderProgram(defaultFrag, defaultVert);
        }

        public override void Draw(GL_ControlLegacy control, Pass pass)
        {
            foreach (var group in PathGroups)
            {
                foreach (var path in group.PathPoints)
                {
                    if (!path.RenderablePoint.IsVisable)
                        continue;

                    GL.LineWidth(2f);
                    foreach (var nextPt in path.NextPoints)
                    {
                        GL.Color3(Color.Blue);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(path.Translate);
                        GL.Vertex3(PathGroups[nextPt.PathID].PathPoints[nextPt.PtID].Translate);
                        GL.End();
                    }
                    foreach (var prevPt in path.PrevPoints)
                    {
                        GL.Color3(Color.Blue);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(path.Translate);
                        GL.Vertex3(PathGroups[prevPt.PathID].PathPoints[prevPt.PtID].Translate);
                        GL.End();
                    }
                }
            }
        }

        public override void Draw(GL_ControlModern control, Pass pass)
        {
            control.CurrentShader = defaultShaderProgram;

            foreach (var group in PathGroups)
            {
                foreach (var path in group.PathPoints)
                {
                    GL.LineWidth(2f);
                    foreach (var nextPt in path.NextPoints)
                    {
                        GL.Color3(Color.Blue);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(path.Translate);
                        GL.Vertex3(PathGroups[nextPt.PathID].PathPoints[nextPt.PtID].Translate);
                        GL.End();
                    }
                    foreach (var prevPt in path.PrevPoints)
                    {
                        GL.Color3(Color.Blue);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(path.Translate);
                        GL.Vertex3(PathGroups[prevPt.PathID].PathPoints[prevPt.PtID].Translate);
                        GL.End();
                    }
                }
            }
        }
    }

}

﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using System.IO;

using Rbx2Source.Reflection;
using Rbx2Source.Geometry;

namespace Rbx2Source.StudioMdl
{
    public class Node : IStudioMdlEntity<Node>
    {
        public string Name;

        public Bone Bone;
        public Mesh Mesh;

        public int NodeIndex;
        public int ParentIndex = -1;
        public bool UseParentIndex = false;
        
        public string GroupName => "nodes";

        private int FindParent(List<Node> nodes, Node node)
        {
            Bone bone = node.Bone;

            BasePart part0 = bone.Part0;
            BasePart part1 = bone.Part1;

            if (part0 != part1)
            {
                Node parent = null;

                foreach (Node n in nodes)
                {
                    Bone b = n.Bone;

                    if (b != bone && b.Part1 == part0)
                    {
                        parent = n;
                        break;
                    }
                }

                return nodes.IndexOf(parent);
            }

            return -1;
        }

        public void WriteStudioMdl(StringWriter fileBuffer, Node node, List<Node> nodes)
        {
            int nodeIndex = nodes.IndexOf(node);
            node.NodeIndex = nodeIndex;

            string nodeName = '"' + node.Name + '"';
            int parentIndex = UseParentIndex ? ParentIndex : FindParent(nodes, node);
            ParentIndex = parentIndex;

            string joined = string.Join(" ", nodeIndex, nodeName, parentIndex);
            fileBuffer.WriteLine(joined);
        }
    }
}

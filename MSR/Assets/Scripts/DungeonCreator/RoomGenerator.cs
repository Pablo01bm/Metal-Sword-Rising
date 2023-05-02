using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations;
    private int roomlengthMin1;
    private int roomlengthMin2;

    public RoomGenerator(int maxIterations, int roomlengthMin1, int roomlengthMin2)
    {
        this.maxIterations = maxIterations;
        this.roomlengthMin1 = roomlengthMin1;
        this.roomlengthMin2 = roomlengthMin2;
    }

    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();
        foreach (var space in roomSpaces)
        {
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset);

            Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornerModifier, roomOffset);

            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);
            listToReturn.Add((RoomNode)space);
                
        }
        return listToReturn;
    }
}
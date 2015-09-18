using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Shapes;

using ORM.DisplayGraph.Components.Entity;
using ORM.DisplayGraph.Components.ModelViewer.ViewModels;

namespace ORM.DisplayGraph.Components.ModelViewer
{
    public class UpdateRelationShips
    {
        public static void RefreshRelationShipPosition(
            LinkDefinition linkDefinition,
            Dictionary<TableDefinition, TableDefinitionControl> tableDefinitionControls,
            Canvas modelViewerContainer,
            Line line)
        {
            var sourceTableDefinition = linkDefinition.Source.TableDefinition;
            var targetTableDefinition = linkDefinition.Target.TableDefinition;
            var sourceTableDefinitionControl = tableDefinitionControls[sourceTableDefinition];
            var targetTableDefinitionControl = tableDefinitionControls[targetTableDefinition];
            var sourceCenterPoint = CalculateTableDefinitionControlCenterPoint(sourceTableDefinitionControl);
            var targetCenterPoint = CalculateTableDefinitionControlCenterPoint(targetTableDefinitionControl);
            var sourceTableCoordinateX = Canvas.GetLeft(sourceTableDefinitionControl);
            var targetTableCoordinateX = Canvas.GetLeft(targetTableDefinitionControl);
            var sourceTableCoordinateY = Canvas.GetTop(sourceTableDefinitionControl);
            var targetTableCoordinateY = Canvas.GetTop(targetTableDefinitionControl);

            line.X1 = sourceTableCoordinateX + sourceCenterPoint.X;
            line.X2 = targetTableCoordinateX + targetCenterPoint.X;
            line.Y1 = sourceTableCoordinateY + sourceCenterPoint.Y;
            line.Y2 = targetTableCoordinateY + targetCenterPoint.Y;
        }

        private static Point CalculateTableDefinitionControlCenterPoint(TableDefinitionControl tableDefinitionControl)
        {
            var result = new Point();
            result.X = (int)tableDefinitionControl.ActualWidth / 2;
            result.Y = (int)tableDefinitionControl.ActualHeight/2;
            return result;
        }
    }
}

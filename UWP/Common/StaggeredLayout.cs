﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WeiPo.Common
{
    public class StaggeredLayout : VirtualizingLayout
    {
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding),
            typeof(Thickness),
            typeof(StaggeredLayout),
            new PropertyMetadata(default(Thickness)));


        public static readonly DependencyProperty DesiredColumnWidthProperty = DependencyProperty.Register(
            nameof(DesiredColumnWidth), typeof(double), typeof(StaggeredLayout), new PropertyMetadata(250D));

        private double _columnWidth;

        public Thickness Padding
        {
            get => (Thickness) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        public double DesiredColumnWidth
        {
            get => (double) GetValue(DesiredColumnWidthProperty);
            set => SetValue(DesiredColumnWidthProperty, value);
        }

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            availableSize.Width = availableSize.Width - Padding.Left - Padding.Right;
            availableSize.Height = availableSize.Height - Padding.Top - Padding.Bottom;

            _columnWidth = Math.Min(DesiredColumnWidth, availableSize.Width);
            var numColumns = (int)Math.Floor(availableSize.Width / _columnWidth);
            _columnWidth = availableSize.Width / numColumns;
            var columnHeights = new double[numColumns];

            for (var i = 0; i < context.ItemCount; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                var child = context.GetOrCreateElementAt(i);
                child.Measure(new Size(_columnWidth, availableSize.Height));
                var elementSize = child.DesiredSize;
                columnHeights[columnIndex] += elementSize.Height;
            }

            var desiredHeight = columnHeights.Max();

            return new Size(availableSize.Width, desiredHeight);
        }


        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            var horizontalOffset = Padding.Left;
            var verticalOffset = Padding.Top;
            var numColumns = (int)Math.Floor(finalSize.Width / _columnWidth);
            horizontalOffset += (finalSize.Width - numColumns * _columnWidth) / 2;

            var columnHeights = new double[numColumns];

            for (var i = 0; i < context.ItemCount; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                var child = context.GetOrCreateElementAt(i);
                var elementSize = child.DesiredSize;

                var elementHeight = elementSize.Height;

                var bounds = new Rect(horizontalOffset + _columnWidth * columnIndex,
                    columnHeights[columnIndex] + verticalOffset, _columnWidth, elementHeight);
                child.Arrange(bounds);

                columnHeights[columnIndex] += elementSize.Height;
            }

            return finalSize;
        }

        private int GetColumnIndex(double[] columnHeights)
        {
            var columnIndex = 0;
            var height = columnHeights[0];
            for (var j = 1; j < columnHeights.Length; j++)
                if (columnHeights[j] < height)
                {
                    columnIndex = j;
                    height = columnHeights[j];
                }

            return columnIndex;
        }
    }
}
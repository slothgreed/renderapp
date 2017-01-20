using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OpenTK;
using RenderApp.GLUtil;
using RenderApp.ViewModel.MathVM;
using RenderApp.ViewModel.AssetVM;
namespace RenderApp.ViewModel
{
    class MaterialItemTemplateSelector : DataTemplateSelector
    {

		public DataTemplate ImageTemplate {get;set;}
		public DataTemplate ComboItemTemplate {get;set;}
		public DataTemplate Vector2Template {get;set;}
		public DataTemplate Vector3Template {get;set;}
		public DataTemplate Vector4Template {get;set;}
		public DataTemplate Matrix3Template {get;set;}
		public DataTemplate Matrix4Template {get;set;}
		public DataTemplate DefaultTemplate {get;set;}
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

			if(item is ImageViewModel)
				return ImageTemplate;
			if(item is ComboItemViewModel)
				return ComboItemTemplate;
			if(item is Vector2ViewModel)
				return Vector2Template;
			if(item is Vector3ViewModel)
				return Vector3Template;
			if(item is Vector4ViewModel)
				return Vector4Template;
			if(item is Matrix3ViewModel)
				return Matrix3Template;
			if(item is Matrix4ViewModel)
				return Matrix4Template;
			return DefaultTemplate;
		}
	}
}
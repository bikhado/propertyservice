//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v8.6.4
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder.Embedded;

namespace Umbraco.Web.PublishedModels
{
	/// <summary>Reply Comment</summary>
	[PublishedModel("replyComment")]
	public partial class ReplyComment : PublishedContentModel
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		public new const string ModelTypeAlias = "replyComment";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<ReplyComment, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public ReplyComment(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Content Reply
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("contentReply")]
		public global::System.Web.IHtmlString ContentReply => this.Value<global::System.Web.IHtmlString>("contentReply");

		///<summary>
		/// ID Reply Comment
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("iDReplyComment")]
		public string IDreplyComment => this.Value<string>("iDReplyComment");

		///<summary>
		/// Name User Reply
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("nameUserReply")]
		public string NameUserReply => this.Value<string>("nameUserReply");

		///<summary>
		/// Time Reply
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("timeReply")]
		public global::System.DateTime TimeReply => this.Value<global::System.DateTime>("timeReply");
	}
}

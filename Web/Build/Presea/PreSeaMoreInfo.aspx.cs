using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaMoreInfo : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{

		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			if (string.IsNullOrEmpty(Request.QueryString["s"]))
			{
				PhoenixToolbar toolbarmain = new PhoenixToolbar();
				toolbarmain.AddButton("Save", "SAVE");
				MenuComment.AccessRights = this.ViewState;
				MenuComment.MenuList = toolbarmain.Show();
			}
			if (!IsPostBack)
			{
				if (Request.QueryString["id"] != null)
				{
					ViewState["id"] = Request.QueryString["id"];
					BindDataEdit(ViewState["id"].ToString());
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void BindDataEdit(string id)
	{
		DataTable dt = new DataTable();
		if (Request.QueryString["type"].ToString() == PhoenixCrewAttachmentType.DOCUMENTS.ToString())
		{
			dt = PhoenixPreSeaTraineeTravelDocument.EditTraineeTravelDocument(null, General.GetNullableInteger(id));
			if (dt.Rows.Count > 0)
			{
				txtComment.Content = dt.Rows[0]["FLDREMARKS"].ToString();
			}
		}
		if (Request.QueryString["type"].ToString() == PhoenixCrewAttachmentType.COURSE.ToString())
		{
			dt = PhoenixPreSeaTraineeCourseAndCertificate.EditTraineeCourseCertificate(Convert.ToInt32(id));
			if (dt.Rows.Count > 0)
			{
				txtComment.Content = dt.Rows[0]["FLDREMARKS"].ToString();
			}
		}

	}


	protected void MenuComment_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				PhoenixPreSeaCommon.UpdatePreSeaRelatedRemarks(int.Parse(Request.QueryString["id"]), Request.QueryString["type"], txtComment.Content);
			}
			string Script = "";
			Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
			Script += "parent.CloseCodeHelpWindow('MoreInfo');";
			Script += "</script>" + "\n";
			Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
}

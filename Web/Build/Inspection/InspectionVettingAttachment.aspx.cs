using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class InspectionVettingAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                if (Request.QueryString["dtkey"].ToString() != "" && Request.QueryString["dtkey"] != null)
                    ViewState["DTKEY"] = Request.QueryString["dtkey"].ToString();
                if (Request.QueryString["VESSELID"].ToString() != "" && Request.QueryString["VESSELID"] != null)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                if (ViewState["DTKEY"] != null)
                {
                    if (Request.QueryString["viewonly"].ToString().Equals("0"))
                    {
                        ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY="
                            + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.QUALITY
                            + "&type=" + rblAttachmentType.SelectedValue
                            + "&VESSELID=" + ViewState["VESSELID"];
                    }
                    else
                    {
                        ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?U=1&DTKEY="
                            + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.QUALITY
                            + "&type=" + rblAttachmentType.SelectedValue
                            + "&VESSELID=" + ViewState["VESSELID"];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetValue(object sender, EventArgs e)
    {
        if (ViewState["DTKEY"] != null)
        {
            if (Request.QueryString["viewonly"].ToString().Equals("0"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY="
                    + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.QUALITY
                    + "&type=" + rblAttachmentType.SelectedValue
                    + "&VESSELID=" + ViewState["VESSELID"];
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?U=1&DTKEY="
                    + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.QUALITY
                    + "&type=" + rblAttachmentType.SelectedValue
                    + "&VESSELID=" + ViewState["VESSELID"];
            }
        }
    }
}

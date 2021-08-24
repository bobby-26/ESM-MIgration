using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegisterAddressAssessmentOptionsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["qid"] = "";
            ViewState["oid"] = "";
            if (Request.QueryString["oid"] != null)
            {
                ViewState["oid"] = Request.QueryString["oid"].ToString();
                ViewState["qid"] = Request.QueryString["qid"].ToString();
                DataTable dt;
                dt = PhoenixAddressAssessmentQuestion.InsterviewAssessmentQuestionList(General.GetNullableInteger(ViewState["qid"].ToString()),null);
                txtquestion.Text = dt.Rows[0]["FLDQUESTION"].ToString();
               
                dt = PhoenixRegistersAddressAssessmentOptions.AssessmentOptionList(General.GetNullableInteger(ViewState["qid"].ToString())
                                                                                             ,General.GetNullableInteger(Request.QueryString["oid"].ToString())
                                                                                             ,null);
                txtOptionName.Text= dt.Rows[0]["FLDOPTIONNAME"].ToString();
                chkactive.Checked = dt.Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;


            }
            else
            {
                ViewState["qid"] = Request.QueryString["qid"].ToString();
                DataTable dt;
                dt = PhoenixAddressAssessmentQuestion.InsterviewAssessmentQuestionList(General.GetNullableInteger(ViewState["qid"].ToString()),null);
                txtquestion.Text = dt.Rows[0]["FLDQUESTION"].ToString();
            }
        }
    }

    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        if (CommandName.ToUpper()=="SAVE")
        {
            if (ViewState["oid"].ToString() == "")
            {
                PhoenixRegistersAddressAssessmentOptions.AssessmentOptionInsert(General.GetNullableInteger(ViewState["qid"].ToString())
                                                                                , txtOptionName.Text
                                                                            , General.GetNullableInteger(chkactive.Checked == true ? "1" : "0"));
            }
            else
            {
                PhoenixRegistersAddressAssessmentOptions.AssessmentOptionUpdate(General.GetNullableInteger(ViewState["qid"].ToString())
                                                                                , General.GetNullableInteger(ViewState["oid"].ToString())
                                                                                , txtOptionName.Text
                                                                            , General.GetNullableInteger(chkactive.Checked == true ? "1" : "0"));
            }
        }

          
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('CI','ifMoreInfo');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
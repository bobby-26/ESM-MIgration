using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data.SqlClient;
using System.Data;
public partial class Registers_RegistersAddressAssessmentQuestionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbar.Show();

        if(!IsPostBack)
        {
            ViewState["qid"] = "";
            if (Request.QueryString["qid"] != null)
            {
                ViewState["qid"] = Request.QueryString["qid"].ToString();
                DataTable dt = PhoenixAddressAssessmentQuestion.InsterviewAssessmentQuestionList(General.GetNullableInteger(ViewState["qid"].ToString()),null);
                txtquestion.Text = dt.Rows[0]["FLDQUESTION"].ToString();
                chkremark.Checked = (dt.Rows[0]["FLDREQUIREREMARK"].ToString() == "0" ? false : true);
                chkactive.Checked = (dt.Rows[0]["FLDISACTIVE"].ToString() == "0" ? false : true);
            }
        }
    }

    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        if(CommandName.ToUpper()=="SAVE")
        {
            if (ViewState["qid"].ToString() == "")
            {
                PhoenixAddressAssessmentQuestion.InterviewAssessmentQuestionInsert(txtquestion.Text
                                                                                    , chkremark.Checked == true ? 1 : 0
                                                                                    , chkactive.Checked == true ? 1 : 0);
            }
            else
            {
                PhoenixAddressAssessmentQuestion.InterviewAssessmentQuestionUpdate(int.Parse(ViewState["qid"].ToString())
                                                                                    , txtquestion.Text
                                                                                    , chkremark.Checked == true ? 1 : 0
                                                                                    , chkactive.Checked == true ? 1 : 0);
            }
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('CI','ifMoreInfo');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }
}
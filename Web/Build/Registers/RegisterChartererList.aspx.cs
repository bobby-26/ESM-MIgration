using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;


public partial class RegisterChartererList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuRankList.AccessRights = this.ViewState;
        MenuRankList.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["DocumentCourseId"] != null && Request.QueryString["type"].ToUpper().Equals("COURSE"))
            {
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["DocumentCourseId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["DocumentLicenseId"] != null && Request.QueryString["type"].ToUpper().Equals("LICENSE"))
            {
                ViewState["DOCUMENTLICENSEID"] = Request.QueryString["DocumentLicenseId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["DocumentMedicalId"] != null && Request.QueryString["type"].ToUpper().Equals("MEDICAL"))
            {
                ViewState["DOCUMENTMEDICALID"] = Request.QueryString["DocumentMedicalId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["OtherDocumentId"] != null && Request.QueryString["type"].ToUpper().Equals("OTHER"))
            {
                ViewState["DOCUMENTOTHERID"] = Request.QueryString["OtherDocumentId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            BindChartererList();
            BindMapping();

        }
    }
    private void BindChartererList()
    {
        DataSet ds = PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixStandardList();
        chkChartererList.DataSource = ds;
        chkChartererList.DataBindings.DataTextField = "FLDNAME";
        chkChartererList.DataBindings.DataValueField = "FLDADDRESSCODE";
        chkChartererList.DataBind();
    }
    private void BindMapping()
    {
        BindChartererList();
        if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
        {

            if (General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    General.RadBindCheckBoxList(chkChartererList, ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString());
                }
            }
        }
        else if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
        {
            if (General.GetNullableInteger(ViewState["DOCUMENTLICENSEID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(Int16.Parse(ViewState["DOCUMENTLICENSEID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    General.RadBindCheckBoxList(chkChartererList, ds.Tables[0].Rows[0]["FLDCHARTER"].ToString());
                }
            }
        }
        else if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
        {
            if (General.GetNullableInteger(ViewState["DOCUMENTMEDICALID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersDocumentMedical.EditDocumentMedical(Int16.Parse(ViewState["DOCUMENTMEDICALID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    General.RadBindCheckBoxList(chkChartererList, ds.Tables[0].Rows[0]["FLDCHARTER"].ToString());
                }
            }
        }
        else if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
        {
            if (General.GetNullableInteger(ViewState["DOCUMENTOTHERID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersDocumentOther.EditDocumentOther(Int16.Parse(ViewState["DOCUMENTOTHERID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    General.RadBindCheckBoxList(chkChartererList, ds.Tables[0].Rows[0]["FLDCHARTER"].ToString());
                }
            }
        }

    }
    protected void MenuRankList_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
            {
                string ChartererList = "";
                foreach (ButtonListItem li in chkChartererList.Items)
                {
                    if (li.Selected)
                    {
                        ChartererList += li.Value + ",";
                    }
                }

                if (ChartererList != "")
                {
                    ChartererList = "," + ChartererList;
                }
                PhoenixRegisterCrewList.UpdateDocumentCourseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()),null, null, ChartererList, null, null);

            }
            else if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
            {
                string ChartererList = "";
                foreach (ButtonListItem li in chkChartererList.Items)
                {
                    if (li.Selected)
                    {
                        ChartererList += li.Value + ",";
                    }
                }

                if (ChartererList != "")
                {
                    ChartererList = "," + ChartererList;
                }

                PhoenixRegisterCrewList.UpdateDocumentLicenseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Int16.Parse(ViewState["DOCUMENTLICENSEID"].ToString()), null,null, ChartererList, null, null);

            }
            else if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
            {
                string ChartererList = "";
                foreach (ButtonListItem li in chkChartererList.Items)
                {
                    if (li.Selected)
                    {
                        ChartererList += li.Value + ",";
                    }
                }

                if (ChartererList != "")
                {
                    ChartererList = "," + ChartererList;
                }

                PhoenixRegisterCrewList.UpdateDocumentMedicalList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Int16.Parse(ViewState["DOCUMENTMEDICALID"].ToString()), null,null, ChartererList, null, null);

            }
            else if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
            {
                string ChartererList = "";
                foreach (ButtonListItem li in chkChartererList.Items)
                {
                    if (li.Selected)
                    {
                        ChartererList += li.Value + ",";
                    }
                }

                if (ChartererList != "")
                {
                    ChartererList = "," + ChartererList;
                }

                PhoenixRegisterCrewList.UpdateDocumentOtherdocumentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Int16.Parse(ViewState["DOCUMENTOTHERID"].ToString()),null,null, ChartererList , null, null);

            }
            BindMapping();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp2', 'codehelp1');", true);

        }
    }
}
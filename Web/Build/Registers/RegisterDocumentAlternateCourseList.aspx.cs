using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using System.Data;
using System.Collections;
using Telerik.Web.UI;
using System;

public partial class Registers_RegisterDocumentAlternateCourseList : PhoenixBasePage
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
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["DocumentLicenseId"].ToString();
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

        }
    }
    public void BindData()
    {
        DataTable dt = PhoenixRegisterDocumentRankGroup.DocumentAlternativeCourseList(General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()), ViewState["TYPE"].ToString());
        gvOwner.DataSource = dt;
    }
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvOwner$ctl00$ctl02$ctl00$chkAllSeal")
        {
            GridHeaderItem headerItem = gvOwner.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllSeal");
            foreach (GridDataItem row in gvOwner.MasterTableView.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                }
            }
        }
    }
    protected void gvOwner_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvOwner_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void MenuRankList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            int index;
            string selectedalternatecourse = "";

            foreach (GridDataItem gvrow in gvOwner.MasterTableView.Items)
            {
                bool result = false;
                index = int.Parse(gvrow.GetDataKeyValue("FLDDOCUMENTID").ToString());

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session

                if (result)
                {
                    selectedalternatecourse += index.ToString() + ",";
                }
            }
            if (selectedalternatecourse != "")
            {
                selectedalternatecourse = "," + selectedalternatecourse;
            }

            if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
            {
                PhoenixRegisterCrewList.UpdateDocumentCourseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), selectedalternatecourse, null);
            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
            {

                PhoenixRegisterCrewList.UpdateDocumentLicenseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, null, null, null, null, null, null, null, null, null, selectedalternatecourse, null);
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp2', 'codehelp1');", true);

        }
    }
}
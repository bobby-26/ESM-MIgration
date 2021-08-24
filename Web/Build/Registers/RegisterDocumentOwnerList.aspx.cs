using System;
using System.Collections.Generic;
using System.Linq;
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

public partial class Registers_RegisterDocumentOwnerList : PhoenixBasePage
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
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["DocumentMedicalId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["OtherDocumentId"] != null && Request.QueryString["type"].ToUpper().Equals("OTHER"))
            {
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["OtherDocumentId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }

        }
    }

    public void BindData()
    {
        DataTable dt = PhoenixRegisterDocumentRankGroup.DocumentOwnerList(General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()), ViewState["TYPE"].ToString());
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
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedRank = new ArrayList();
        int index;
        foreach (GridDataItem gvrow in gvOwner.MasterTableView.Items)
        {
            bool result = false;
            index = int.Parse(gvOwner.MasterTableView.Items[0].GetDataKeyValue("FLDADDRESSCODE").ToString());

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session          
            if (result)
            {
                if (!SelectedRank.Contains(index))
                    SelectedRank.Add(index);
            }
            else
                SelectedRank.Remove(index);
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
            string selectedowner = "";

            foreach (GridDataItem gvrow in gvOwner.MasterTableView.Items)
            {
                bool result = false;
                index = int.Parse(gvrow.GetDataKeyValue("FLDADDRESSCODE").ToString());

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session

                if (result)
                {
                    selectedowner += index.ToString() + ",";
                }
            }
            if (selectedowner != "")
            {
                selectedowner = "," + selectedowner;
            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
            {
                PhoenixRegisterCrewList.UpdateDocumentCourseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, null, null, null, null, null,
                          null, null, null, selectedowner);
                PhoenixRegisterCrewList.UpdateDocumentActualVesselList(int.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), 1);

            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
            {

                PhoenixRegisterCrewList.UpdateDocumentLicenseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, null, null, null, null, null, null, null, null, selectedowner, null, null);
            }

            if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
            {
                PhoenixRegisterCrewList.UpdateDocumentMedicalList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, null, null, null, null, null, null, null, null, selectedowner, null);
            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
            {
                PhoenixRegisterCrewList.UpdateDocumentOtherdocumentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, null, null, null, null, null, null, null, null, selectedowner, null);
            }
        }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp2', 'codehelp1');", true);

        }
    }

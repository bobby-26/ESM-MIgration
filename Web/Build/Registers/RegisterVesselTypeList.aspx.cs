using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegisterVesselTypeList : PhoenixBasePage
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

            //BindVesselTypeList();
            //BindVesselSubTypeList();
            //BindMapping();
            //BindExcludeVesselSubType();
        }
    }

    protected void MenuRankList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            int index;
            string selectedvesselmain = "";
            ArrayList subtypelist = new ArrayList();
            string FinalvesselsubList = "";
            string VesselTypeExcludeList = "";
            foreach (GridDataItem gvrow in gvvesseltype.MasterTableView.Items)
            {
                bool result = false;
                index = int.Parse(gvrow.GetDataKeyValue("FLDVESSELTYPEID").ToString());

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session

                if (result)
                {
                    selectedvesselmain += index.ToString() + ",";
                    DataTable dt1 = PhoenixRegisterDocumentRankGroup.MappedVesselSubTypeWithVesselType(index);
                    for (int j = 0; j <= dt1.Rows.Count - 1; j++)
                    {
                        subtypelist.Add(dt1.Rows[j]["FLDVESSELTYPEID"].ToString());
                    }
                    //excluded ranks
                    RadComboBox chkRankList = (RadComboBox)gvrow.FindControl("chkRankList");
                    if (chkRankList != null)
                    {
                        foreach (RadComboBoxItem li in chkRankList.Items)
                        {
                            if (li.Checked==false)
                            {
                                subtypelist.Remove(li.Value);
                                VesselTypeExcludeList += li.Value + ",";
                            }
                        }
                    }
                }


            }
            //
            if (selectedvesselmain != "")
            {
                selectedvesselmain = "," + selectedvesselmain;
            }
            if (subtypelist != null && subtypelist.Count > 0)
            {
                foreach (string index1 in subtypelist)
                { FinalvesselsubList = FinalvesselsubList + index1 + ","; }
            }
            if (FinalvesselsubList != "")
                FinalvesselsubList = "," + FinalvesselsubList;
            if (VesselTypeExcludeList != "")
                VesselTypeExcludeList = ',' + VesselTypeExcludeList;
            if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
            {
                PhoenixRegisterCrewList.UpdateDocumentCourseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                           , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, FinalvesselsubList, null, null, null, null,
                           VesselTypeExcludeList, null, selectedvesselmain, null);

                PhoenixRegisterCrewList.UpdateDocumentActualVesselList(int.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), 1);
                
            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
            {
                PhoenixRegisterCrewList.UpdateDocumentLicenseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, FinalvesselsubList, null, null, null, null, VesselTypeExcludeList, null, selectedvesselmain, null, null, null);
            }

            if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
            {
                PhoenixRegisterCrewList.UpdateDocumentMedicalList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, FinalvesselsubList, null, null, null, null, VesselTypeExcludeList, null, selectedvesselmain, null, null);
            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
            {
                PhoenixRegisterCrewList.UpdateDocumentOtherdocumentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, FinalvesselsubList, null, null, null, null, VesselTypeExcludeList, null, selectedvesselmain, null, null);
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp2', 'codehelp1');", true);

        }
    }
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvvesseltype$ctl00$ctl02$ctl00$chkAllSeal")
        {
            GridHeaderItem headerItem = gvvesseltype.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllSeal");
            foreach (GridDataItem row in gvvesseltype.MasterTableView.Items)
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
                RadComboBox chkRankList1 = (RadComboBox)row.FindControl("chkRankList");
                if (chkRankList1 != null)
                {
                    foreach (RadComboBoxItem gvrow in chkRankList1.Items)
                    {
                        if (cbSelected.Checked == true)
                            gvrow.Checked = true;
                        else
                            gvrow.Checked = false;
                    }

                }
            }
        }
    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedRank = new ArrayList();
        int index;
        foreach (GridDataItem gvrow in gvvesseltype.MasterTableView.Items)
        {
            bool result = false;
            index = int.Parse(gvvesseltype.MasterTableView.Items[0].GetDataKeyValue("FLDVESSELTYPEID").ToString());

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

    //private void BindVesselSubTypeList()
    //{
    //    DataSet ds = PhoenixRegistersVesselType.ListDocCourseVesselType(0);
    //    chkVesselsubTypeList.DataSource = ds;
    //    chkVesselsubTypeList.DataTextField = "FLDTYPEDESCRIPTION";
    //    chkVesselsubTypeList.DataValueField = "FLDVESSELTYPEID";
    //    chkVesselsubTypeList.DataBind();
    //}

    //private void BindVesselTypeList()
    //{
    //    DataSet ds = PhoenixRegistersHard.ListHard(1, 81);//  PhoenixRegistersVesselType.ListDocCourseVesselType(0);
    //    chkVesselTypeList.DataSource = ds;
    //    chkVesselTypeList.DataBindings.DataTextField = "FLDHARDNAME";
    //    chkVesselTypeList.DataBindings.DataValueField = "FLDHARDCODE";
    //    chkVesselTypeList.DataBind();
    //}

    public void BindData()
    {
        DataTable dt = PhoenixRegisterDocumentRankGroup.DocumentVesselTypeList(General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()), ViewState["TYPE"].ToString());
        gvvesseltype.DataSource = dt;
    }

    protected void gvvesseltype_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRankGroup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadComboBox chkRankList = (RadComboBox)e.Item.FindControl("chkRankList");
        RadLabel lblgroupid = (RadLabel)e.Item.FindControl("lblgroupid");
        RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");

        if (chkRankList != null)
        {
            DataTable dt = new DataTable();
            dt = PhoenixRegisterDocumentRankGroup.MappedVesselSubTypeWithVesselType(General.GetNullableInteger(lblgroupid.Text));
            chkRankList.DataSource = dt;
            chkRankList.DataTextField = "FLDTYPEDESCRIPTION";
            chkRankList.DataValueField = "FLDVESSELTYPEID";
            chkRankList.DataBind();
            //chkRankList.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            General.RadBindComboBoxCheckList(chkRankList, DataBinder.Eval(e.Item.DataItem, "FLDVESSELTYPES").ToString());
        }
    }

    protected void gvvesseltype_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SELECT")
        {
            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
            RadComboBox chkRankList1 = (RadComboBox)e.Item.FindControl("chkRankList");
            if (chkRankList1 != null)
            {
                foreach (RadComboBoxItem gvrow in chkRankList1.Items)
                {
                    if (chkSelect.Checked == true)
                        gvrow.Checked = true;
                    else
                        gvrow.Checked = false;
                }

            }
        }
    }
}
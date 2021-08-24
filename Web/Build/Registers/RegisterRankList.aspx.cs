using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Collections;
using System.IO;

public partial class RegisterRankList : PhoenixBasePage
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

           
            //BindRankGroupList();
            //BindRank();
            // BindMapping();
            // BindExcludeRank();
        }
        if (ViewState["TYPE"].ToString() == "MEDICAL")
        {
            gvRankGroup.MasterTableView.GetColumn("FLDNEWHANDAGE").Visible = true;
            gvRankGroup.MasterTableView.GetColumn("FLDEXHANDAGE").Visible = true;

        }
    }
    protected void gvRankGroup_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if(e.CommandName.ToUpper()=="SELECT")
        {
            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
            RadComboBox chkRankList1 = (RadComboBox)e.Item.FindControl("chkRankList");
            if(chkRankList1!=null)
            {
                foreach (RadComboBoxItem gvrow in chkRankList1.Items)
                {
                    if(chkSelect.Checked==true)
                        gvrow.Checked = true;
                    else
                        gvrow.Checked = false;
                }
                   
            }
        }


    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvRankGroup$ctl00$ctl02$ctl00$chkAllSeal")
        {
            GridHeaderItem headerItem = gvRankGroup.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllSeal");
            foreach (GridDataItem row in gvRankGroup.MasterTableView.Items)
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
        foreach (GridDataItem gvrow in gvRankGroup.MasterTableView.Items)
        {
            bool result = false;
            index = int.Parse(gvRankGroup.MasterTableView.Items[0].GetDataKeyValue("FLDGROUPRANKID").ToString());

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session          
            if (result)
            {
                DataTable dt1 = PhoenixRegisterDocumentRankGroup.MappedRankWithGroupRankList(index);
                RadComboBox chkRankList1 = (RadComboBox)gvrow.FindControl("chkRankList");
                //for (int j = 0; j <= dt1.Rows.Count - 1; j++)
                //{
                //    chkRankList1.DataSource = dt1;
                //    chkRankList1.DataBind();
                //    chkRankList1.Items[j].Checked = true;

                //}
                //if (!SelectedRank.Contains(index))
                //    SelectedRank.Add(index);
            }
            else
                SelectedRank.Remove(index);
        }

    }
    public void BindData()
    {
        DataTable dt = PhoenixRegisterDocumentRankGroup.DocumentGroupRankList(General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()), ViewState["TYPE"].ToString());
        gvRankGroup.DataSource = dt;
    }
    //private void BindRankGroupList()
    //{
    //    chkRankGroupList.DataSource = PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO");
    //    chkRankGroupList.DataBindings.DataTextField = "FLDHARDNAME";
    //    chkRankGroupList.DataBindings.DataValueField = "FLDHARDCODE";
    //    chkRankGroupList.DataBind();

    //}
    //public void BindRank()
    //{
    //    DataSet ds = PhoenixRegistersRank.ListRank();
    //    chkRankList.DataSource = ds;
    //    chkRankList.DataTextField = "FLDRANKNAME";
    //    chkRankList.DataValueField = "FLDRANKID";
    //    chkRankList.DataBind();
    //}

    //public void BindExcludeRank()
    //{
    //    if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
    //    {
    //        if (General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()) != null)
    //        {
    //            DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {

    //                // General.RadBindComboBoxCheckList(chkRankList, ds.Tables[0].Rows[0]["FLDRANKEXCLUDE"].ToString());
    //            }
    //        }
    //    }


    //}
    //private void BindMapping()
    //{
    //    //BindRankGroupList();
    //    if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
    //    {
    //        if (General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()) != null)
    //        {
    //            DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {

    //                // General.RadBindCheckBoxList(chkRankGroupList, ds.Tables[0].Rows[0]["FLDRANK"].ToString());
    //            }
    //        }
    //    }
    //    else if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
    //    {
    //        if (General.GetNullableInteger(ViewState["DOCUMENTLICENSEID"].ToString()) != null)
    //        {
    //            DataSet ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(Int16.Parse(ViewState["DOCUMENTLICENSEID"].ToString()));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {

    //                // General.RadBindCheckBoxList(chkRankGroupList, ds.Tables[0].Rows[0]["FLDRANKS"].ToString());
    //            }
    //        }
    //    }
    //    else if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
    //    {
    //        if (General.GetNullableInteger(ViewState["DOCUMENTMEDICALID"].ToString()) != null)
    //        {
    //            DataSet ds = PhoenixRegistersDocumentMedical.EditDocumentMedical(Int16.Parse(ViewState["DOCUMENTMEDICALID"].ToString()));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                // General.RadBindCheckBoxList(chkRankGroupList, ds.Tables[0].Rows[0]["FLDRANKS"].ToString());
    //            }
    //        }
    //    }
    //    else if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
    //    {
    //        if (General.GetNullableInteger(ViewState["DOCUMENTOTHERID"].ToString()) != null)
    //        {
    //            DataSet ds = PhoenixRegistersDocumentOther.EditDocumentOther(Int16.Parse(ViewState["DOCUMENTOTHERID"].ToString()));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                //  General.RadBindCheckBoxList(chkRankGroupList, ds.Tables[0].Rows[0]["FLDRANKS"].ToString());
    //            }
    //        }
    //    }

    //}

    protected void MenuRankList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            int index;
            string selectedgroup = "";
            ArrayList ranklists = new ArrayList();
            string FinalRankList = "";
            string RankExcludeList = "";
            foreach (GridDataItem gvrow in gvRankGroup.MasterTableView.Items)
            {
                bool result = false;
                index = int.Parse(gvrow.GetDataKeyValue("FLDGROUPRANKID").ToString());

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session

                if (result)
                {
                    selectedgroup += index.ToString() + ",";
                    DataTable dt1 = PhoenixRegisterDocumentRankGroup.MappedRankWithGroupRankList(index);
                    RadComboBox chkRankList1 = (RadComboBox)gvrow.FindControl("chkRankList");
                    for (int j = 0; j <= dt1.Rows.Count - 1; j++)
                    {
                        
                        ranklists.Add(dt1.Rows[j]["FLDRANKID"].ToString());
                       // chkRankList1.CheckedItems[j].Checked = true;
                        
                    }
                    //excluded ranks
                    RadComboBox chkRankList = (RadComboBox)gvrow.FindControl("chkRankList");
                    if (chkRankList != null)
                    {
                        foreach (RadComboBoxItem li in chkRankList.Items)
                        {
                            if (li.Checked==false)
                            {
                                ranklists.Remove(li.Value);
                                RankExcludeList += li.Value + ",";
                            }
                        }
                    }
                }


            }
            //
            if (selectedgroup != "")
            {
                selectedgroup = "," + selectedgroup;
            }
            if (ranklists != null && ranklists.Count > 0)
            {
                foreach (string index1 in ranklists)
                { FinalRankList = FinalRankList + index1 + ","; }
            }
            if (FinalRankList != "")
                FinalRankList = "," + FinalRankList;
            if (RankExcludeList != "")
                RankExcludeList = ',' + RankExcludeList;

            if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
            {
                PhoenixRegisterCrewList.UpdateDocumentCourseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                           , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), FinalRankList, null, null, null, null, RankExcludeList, null, selectedgroup, null, null);
            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
            {
                PhoenixRegisterCrewList.UpdateDocumentLicenseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                  , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), FinalRankList.ToString(), null, null, null, null, RankExcludeList, null, selectedgroup, null, null, null, null);
            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
            {
                PhoenixRegisterCrewList.UpdateDocumentMedicalList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), FinalRankList.ToString(), null, null, null, null, RankExcludeList, null, selectedgroup, null, null, null);
                bool result;
                result = true;
                InsertAppraisalProfileBulk(ref result);
            }
            if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
            {
                PhoenixRegisterCrewList.UpdateDocumentOtherdocumentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), FinalRankList, null, null, null, null, RankExcludeList, null, selectedgroup, null, null, null);
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp2', 'codehelp1');", true);

        }
    }
    //protected void MenuRankList_TabStripCommand(object sender, EventArgs e)
    //{

    //    RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //    string CommandName = ((RadToolBarButton)dce.Item).CommandName;
    //    if (CommandName.ToUpper().Equals("SAVE"))
    //    {
    //        if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
    //        {
    //            string RankList = "";
    //            foreach (ButtonListItem li in chkRankGroupList.Items)
    //            {
    //                if (li.Selected)
    //                {
    //                    RankList += li.Value + ",";
    //                }
    //            }
    //            if (RankList != "")
    //            {
    //                RankList = "," + RankList;
    //            }

    //            string rankexclude = "";
    //            foreach (RadComboBoxItem li in chkRankList.Items)
    //            {
    //                if (li.Selected)
    //                {
    //                    rankexclude += li.Value + ",";
    //                }
    //            }
    //            if (rankexclude != "")
    //            {
    //                rankexclude = "," + rankexclude;
    //            }
    //            PhoenixRegisterCrewList.UpdateDocumentCourseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                        , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), RankList, null, null, null, null,rankexclude,null);


    //        }
    //        else if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
    //        {
    //            string RankList = "";
    //            foreach (ButtonListItem li in chkRankGroupList.Items)
    //            {
    //                if (li.Selected)
    //                {
    //                    RankList += li.Value + ",";
    //                }
    //            }
    //            if (RankList != "")
    //            {
    //                RankList = "," + RankList;
    //            }

    //            PhoenixRegisterCrewList.UpdateDocumentLicenseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , Int16.Parse(ViewState["DOCUMENTLICENSEID"].ToString()), RankList.ToString(), null, null, null, null);

    //        }
    //        else if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
    //        {
    //            string RankList = "";
    //            foreach (ButtonListItem li in chkRankGroupList.Items)
    //            {
    //                if (li.Selected)
    //                {
    //                    RankList += li.Value + ",";
    //                }
    //            }
    //            if (RankList != "")
    //            {
    //                RankList = "," + RankList;
    //            }

    //            PhoenixRegisterCrewList.UpdateDocumentMedicalList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , Int16.Parse(ViewState["DOCUMENTMEDICALID"].ToString()), RankList.ToString(), null, null, null, null);

    //        }
    //        else if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
    //        {
    //            string RankList = "";
    //            foreach (ButtonListItem li in chkRankGroupList.Items)
    //            {
    //                if (li.Selected)
    //                {
    //                    RankList += li.Value + ",";
    //                }
    //            }
    //            if (RankList != "")
    //            {
    //                RankList = "," + RankList;
    //            }

    //            PhoenixRegisterCrewList.UpdateDocumentOtherdocumentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , Int16.Parse(ViewState["DOCUMENTOTHERID"].ToString()), RankList.ToString(), null, null, null, null);

    //        }
    //        BindMapping();


    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp2', 'codehelp1');", true);

    //    }
    //}




    protected void gvRankGroup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRankGroup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadComboBox chkRankList = (RadComboBox)e.Item.FindControl("chkRankList");
        RadLabel lblgroupid = (RadLabel)e.Item.FindControl("lblgroupid");
        RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");

        UserControlDecimal ucnewhandage = (UserControlDecimal)e.Item.FindControl("ucnewhandage");
        UserControlDecimal ucexhandage = (UserControlDecimal)e.Item.FindControl("ucexhandage");

        if (chkRankList != null)
        {
            DataTable dt = new DataTable();
            dt = PhoenixRegisterDocumentRankGroup.MappedRankWithGroupRankList(General.GetNullableInteger(lblgroupid.Text));
            chkRankList.DataSource = dt;
            chkRankList.DataTextField = "FLDRANKNAME";
            chkRankList.DataValueField = "FLDRANKID";
            chkRankList.DataBind();
           // chkRankList.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            General.RadBindComboBoxCheckList(chkRankList, DataBinder.Eval(e.Item.DataItem, "FLDRANKS").ToString());
        }
       
    }
    private void InsertAppraisalProfileBulk(ref bool result)
    {
        result = true;

        DataSet ds = new DataSet();

        DataTable table = new DataTable();
        table.Columns.Add("FLDMAPPINGID", typeof(Guid));
        table.Columns.Add("FLDDOCUMENTMEDICALID", typeof(int));
        table.Columns.Add("FLDGROUPRANK", typeof(int));
        table.Columns.Add("FLDNEWHANDAGE", typeof(int));
        table.Columns.Add("FLDEXHANDAGE", typeof(int));

        int count = 0, i = 0;
        count = gvRankGroup.Items.Count;
        
        foreach (GridDataItem gv in gvRankGroup.MasterTableView.Items)
        {
            if (count > i && gv.Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
            {
                string ucnewhandage = "";
                string ucexhandage = "";
                ucnewhandage = ((UserControlDecimal)gv.FindControl("ucnewhandage")).Text.TrimStart('_').ToString();
                ucexhandage = ((UserControlDecimal)gv.FindControl("ucexhandage")).Text.TrimStart('_').ToString();

                if ((ucexhandage != "" && int.Parse(ucexhandage)>0)|| (ucnewhandage != "" && int.Parse(ucnewhandage) > 0))
                {
                    table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblmappingid")).Text),
                         General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()),
                            General.GetNullableInteger(((RadLabel)gv.FindControl("lblgroupid")).Text),                           
                            General.GetNullableInteger(ucnewhandage),
                            General.GetNullableInteger(ucexhandage)
                               );
                }
               
                i++;
            }
        }
     

        ds.Tables.Add(table);

        StringWriter sw = new StringWriter();
        ds.WriteXml(sw);
        string resultstring = sw.ToString();

        PhoenixRegisterDocumentRankGroup.InsertAgerMappingBulk(int.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), resultstring);
    }
}


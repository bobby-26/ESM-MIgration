using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
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
public partial class CrewOffshore_CrewOffshoreSTCWTrainingMatrix : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        confirm.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        DataSet ds = PhoenixRegistersVesselType.ListVesselType(0);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                toolbar.AddButton(ds.Tables[0].Rows[i]["FLDVESSELTYPECODE"].ToString(), ds.Tables[0].Rows[i]["FLDVESSELTYPEID"].ToString(), ToolBarDirection.Right);
        }
        MenuVesselTypeList.AccessRights = this.ViewState;
        MenuVesselTypeList.MenuList = toolbar.Show();

        PhoenixToolbar toolbardoc = new PhoenixToolbar();
        toolbardoc.AddButton("Charters Requirement", "CHARTER", ToolBarDirection.Right);
        toolbardoc.AddButton("Company Requirement", "COMPANY", ToolBarDirection.Right);
        toolbardoc.AddButton("Flag State", "FLAG", ToolBarDirection.Right);
        toolbardoc.AddButton("STCW", "STCW", ToolBarDirection.Right);
        //MenuDocTypeList.AccessRights = this.ViewState;
        //MenuDocTypeList.MenuList = toolbardoc.Show();


        if (!IsPostBack)
        {
            if (Request.QueryString["VESSELTYPE"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELTYPE"].ToString();

                rblFormat.SelectedIndex = 0;
                ViewState["FORMTYPE"] = rblFormat.SelectedValue.ToString();
                EditDetails();


            }
        }

    }

    public void EditDetails()
    {
        if (ViewState["VESSELID"] != null)
        {
            DataSet ds1 = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["VESSELID"].ToString()));
            ViewState["VESSELTYPE"] = ds1.Tables[0].Rows[0]["FLDTYPE"].ToString();
            lbltypehead.Text = ds1.Tables[0].Rows[0]["FLDVESSELTYPE"].ToString();
        }
    }
    protected void MenuVesselTypeList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        ViewState["VESSELTYPE"] = CommandName.ToString();
        rblFormat.SelectedIndex = 0;
        ViewState["FORMTYPE"] = rblFormat.SelectedValue.ToString();
        lblvalue.Visible = false;
        ddlvalue.Items.Clear();
        ddlvalue.Visible = false;
        DataSet ds1 = PhoenixRegistersVesselType.EditVesselType(int.Parse(ViewState["VESSELTYPE"].ToString()), 0);
        lbltypehead.Text = ds1.Tables[0].Rows[0]["FLDTYPEDESCRIPTION"].ToString();
        BindData();
        gvmatrixView.Rebind();

    }

    protected void confirm_Click(object sender, EventArgs e)
    {

    }

    protected void MenuDocTypeList_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void gvmatrixView_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvmatrixView_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        DataSet ds = (DataSet)gv.DataSource;
        if (e.Item is GridDataItem)
        {
            if (ds.Tables[2].Columns.Count > 0)
            {
                string tooltip = string.Empty;
                string docid = ((Label)e.Item.FindControl("lbldocid")).Text;// drv["FLDDOCUMENTID"].ToString();
                string doctype = ((Label)e.Item.FindControl("lbldoctype")).Text;// drv["FLDDOCUMENTID"].ToString();lblcode
                string lblcode = ((Label)e.Item.FindControl("lblcode")).Text;
                DataTable header = ds.Tables[1];
                DataTable dtdocs = ds.Tables[0];
                DataTable dtvalues = ds.Tables[2];

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] drdetails = dtvalues.Select("FLDDOCUMENTID = " + docid +
                                                " AND FLDDOCMENTTYPE = " + doctype +
                                                " AND  FLDRANKHEADING = '" + header.Rows[i]["FLDHEADINGNAME"].ToString() + "'" +
                                                " AND  FLDOCCODE = '" + lblcode + "'");

                    RadCheckBox chkdetails = new RadCheckBox();
                    Label lblTQMatrix = new Label();
                    Label lblrankid = new Label();
                    chkdetails.Visible = true;
                    if (Convert.ToInt16(drdetails[0]["FLDHAVING"].ToString()) > 0)
                        chkdetails.Checked = true;//.Text = drdetails[0]["FLDHAVING"].ToString();                    
                    else
                        chkdetails.Checked = false;

                    chkdetails.AutoPostBack = true;
                    chkdetails.CommandName = "SELECT";
                    chkdetails.CheckedChanged += new EventHandler(ck_CheckedChanged);
                    //chkdetails.Attributes.Add("onchange", "submitit(this);");

                    lblTQMatrix.Text = drdetails[0]["FLDTQID"].ToString();
                    lblrankid.Text = drdetails[0]["FLDRANKID"].ToString();



                    e.Item.Cells[i + 4].Attributes.Add("lblTQMatrix", lblTQMatrix.Text);
                    e.Item.Cells[i + 4].Controls.Add(chkdetails);
                    e.Item.Cells[i + 4].Attributes.Add("lblrankid", lblrankid.Text);

                }
            }
        }
    }
    protected void gvmatrixView_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                string lbldocid = ((Label)e.Item.FindControl("lbldocid")).Text;
                string lblcode = ((Label)e.Item.FindControl("lblcode")).Text;

                DataSet ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixDocumentAllRankList(lblcode, General.GetNullableInteger(lbldocid));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string rankid = ((GridTableCell)(((RadCheckBox)e.CommandSource).Parent)).Attributes["lblrankid"];
                    string flag = "";
                    string charter = "";
                    string vesseltype = ds.Tables[0].Rows[0]["FLDVESSELTYPES"].ToString() + ViewState["VESSELTYPE"].ToString() + ",";
                    string ranks = "";
                    string company = ds.Tables[0].Rows[0]["FLDCOMPANIES"].ToString();


                    if (((bool)ViewState["CHECKED"]) == true)
                    {

                        if (ds.Tables[0].Rows[0]["FLDRANKS"].ToString().Contains("," + rankid + ",") == true)
                            ranks = ds.Tables[0].Rows[0]["FLDRANKS"].ToString();
                        else
                            ranks = ds.Tables[0].Rows[0]["FLDRANKS"].ToString() + rankid + ",";

                        if (rblFormat.SelectedIndex == 0) //STCW then update rank and vesseltype
                        {
                            PhoenixCrewOffshoreTrainingMatrix.DocumnetsTrainingMatrixUpdate(
                                null, null, vesseltype, ranks, null, lblcode, General.GetNullableInteger(lbldocid));
                        }
                        if (rblFormat.SelectedIndex == 1) //flag then update rank, vesseltype and flag
                        {
                            if (ds.Tables[0].Rows[0]["FLDFLAG"].ToString().Contains("," + ddlvalue.SelectedValue + ",") == true)
                                flag = ds.Tables[0].Rows[0]["FLDFLAG"].ToString();
                            else
                                flag = ds.Tables[0].Rows[0]["FLDFLAG"].ToString() + ddlvalue.SelectedValue + ",";

                            PhoenixCrewOffshoreTrainingMatrix.DocumnetsTrainingMatrixUpdate(
                                flag, null, vesseltype, ranks, null, lblcode, General.GetNullableInteger(lbldocid));
                        }
                        if (rblFormat.SelectedIndex == 2) //charter then update rank, vesseltype and charter
                        {
                            if (ds.Tables[0].Rows[0]["FLDCHARTER"].ToString().Contains("," + ddlvalue.SelectedValue + ",") == true)
                                charter = ds.Tables[0].Rows[0]["FLDCHARTER"].ToString();
                            else
                                charter = ds.Tables[0].Rows[0]["FLDCHARTER"].ToString() + ddlvalue.SelectedValue + ",";

                            PhoenixCrewOffshoreTrainingMatrix.DocumnetsTrainingMatrixUpdate(
                                null, charter, vesseltype, ranks, null, lblcode, General.GetNullableInteger(lbldocid));
                        }
                        if (rblFormat.SelectedIndex == 3) //company then update rank, vesseltype and company
                        {
                            if (ds.Tables[0].Rows[0]["FLDCOMPANIES"].ToString().Contains("," + ddlvalue.SelectedValue + ",") == true)
                                company = ds.Tables[0].Rows[0]["FLDCOMPANIES"].ToString();
                            else
                                company = ds.Tables[0].Rows[0]["FLDCOMPANIES"].ToString() + ddlvalue.SelectedValue + ",";

                            PhoenixCrewOffshoreTrainingMatrix.DocumnetsTrainingMatrixUpdate(
                                null, null, vesseltype, ranks, company, lblcode, General.GetNullableInteger(lbldocid));
                        }
                    }
                    if (((bool)ViewState["CHECKED"]) == false)
                    {
                        string oldtext = "," + rankid + ",";
                        string oldvalue = "," + ddlvalue.SelectedValue.ToString() + ",";
                        ranks = ds.Tables[0].Rows[0]["FLDRANKS"].ToString();
                        ranks = ranks.Replace(oldtext, ",");

                        if (rblFormat.SelectedIndex == 0) //STCW then update rank and vesseltype
                        {

                            PhoenixCrewOffshoreTrainingMatrix.DocumnetsTrainingMatrixUpdate(
                                null, null, vesseltype, ranks, null, lblcode, General.GetNullableInteger(lbldocid));
                        }
                        if (rblFormat.SelectedIndex == 1) //flag then update rank and vesseltype
                        {
                            flag = ds.Tables[0].Rows[0]["FLDFLAG"].ToString();
                            //flag = flag.Replace(oldvalue, ",");
                            PhoenixCrewOffshoreTrainingMatrix.DocumnetsTrainingMatrixUpdate(
                                flag, null, vesseltype, ranks, null, lblcode, General.GetNullableInteger(lbldocid));
                        }
                        if (rblFormat.SelectedIndex == 2) //charter then update rank and vesseltype
                        {
                            charter = ds.Tables[0].Rows[0]["FLDCHARTER"].ToString();
                            //charter = flag.Replace(oldvalue, ",");
                            PhoenixCrewOffshoreTrainingMatrix.DocumnetsTrainingMatrixUpdate(
                                null, charter, vesseltype, ranks, null, lblcode, General.GetNullableInteger(lbldocid));
                        }
                        if (rblFormat.SelectedIndex == 3) //company then update rank and vesseltype
                        {
                            company = ds.Tables[0].Rows[0]["FLDCOMPANIES"].ToString();
                            // company = flag.Replace(oldvalue, ",");
                            PhoenixCrewOffshoreTrainingMatrix.DocumnetsTrainingMatrixUpdate(
                                null, null, vesseltype, ranks, company, lblcode, General.GetNullableInteger(lbldocid));
                        }

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
    protected void ck_CheckedChanged(object sender, EventArgs e)
    {

        string cbID = ((RadCheckBox)sender).ClientID;
        ViewState["CHECKED"] = ((RadCheckBox)sender).Checked;

    }

    private void BindData()
    {
        //int year, month;
        DataSet ds = null;


        try
        {

            if (ViewState["VESSELTYPE"] != null)
            {
                string flag = "";
                string charter = "";
                string company = "";
                if (rblFormat.SelectedIndex == 0)
                {

                }
                else if (rblFormat.SelectedIndex == 1)
                {
                    flag = ddlvalue.SelectedValue.ToString();
                }
                else if (rblFormat.SelectedIndex == 2)
                {
                    charter = ddlvalue.SelectedValue.ToString();
                }
                else if (rblFormat.SelectedIndex == 3)
                {
                    company = ddlvalue.SelectedValue.ToString();
                }
                ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixVesselTypeAllRankList(General.GetNullableInteger(ViewState["VESSELTYPE"].ToString())
                        , General.GetNullableInteger(charter), General.GetNullableInteger(flag), General.GetNullableInteger(company)
                        , General.GetNullableInteger(ViewState["FORMTYPE"].ToString()));
                if (gvmatrixView.Columns.Count < ds.Tables[1].Rows.Count)
                {
                    if (ds.Tables.Count > 0)
                    {


                        DataTable dt = ds.Tables[1];
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            //adding columns dynamically

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                GridBoundColumn field = new GridBoundColumn();
                                field.HeaderStyle.Width = 65;
                                field.HeaderText = General.GetNullableString(dt.Rows[i]["FLDHEADINGNAME"].ToString());
                                field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;

                                gvmatrixView.Columns.Insert(gvmatrixView.Columns.Count, field);
                            }
                        }
                        gvmatrixView.DataSource = ds;

                        ViewState["EDIT"] = "1";
                    }
                    else
                    {
                        gvmatrixView.DataSource = ds;
                    }


                }
                else
                {
                    gvmatrixView.DataSource = ds;

                }
            }
            else { gvmatrixView.DataSource = ds; }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void confirm_Click1(object sender, EventArgs e)
    {

    }

    protected void CrewTrainingMenu_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void rblFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["FORMTYPE"] = rblFormat.SelectedValue.ToString();
        //Value = "2" Text = "STCW" />
        //  Value = "5" Text = "Flag" />
        //    Value = "4" Text = "Charter Requirement" />
        //      Value = "3" Text = "Company Requirement" />
        if (rblFormat.SelectedIndex == 0)
        {
            lblvalue.Visible = false;
            ddlvalue.Items.Clear();
            ddlvalue.Visible = false;
        }
        else if (rblFormat.SelectedIndex == 1)
        {
            lblvalue.Visible = true;
            ddlvalue.Visible = true;
            ddlvalue.DataSource = PhoenixRegistersFlag.ListFlag(null);
            ddlvalue.DataTextField = "FLDFLAGNAME";
            ddlvalue.DataValueField = "FLDCOUNTRYCODE";
            ddlvalue.DataBind();
        }
        else if (rblFormat.SelectedIndex == 2)
        {
            lblvalue.Visible = true;
            ddlvalue.Visible = true;
            DataSet ds = PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixStandardList();
            ddlvalue.DataSource = ds;
            ddlvalue.DataTextField = "FLDNAME";
            ddlvalue.DataValueField = "FLDADDRESSCODE";
            ddlvalue.DataBind();
        }
        else if (rblFormat.SelectedIndex == 3)
        {
            lblvalue.Visible = true;
            ddlvalue.Visible = true;
            DataSet ds = PhoenixRegistersAddress.ListAddress(((int)PhoenixAddressType.MANAGER).ToString());
            ddlvalue.DataSource = ds;
            ddlvalue.DataTextField = "FLDNAME";
            ddlvalue.DataValueField = "FLDADDRESSCODE";
            ddlvalue.DataBind();
        }
        BindData();
        gvmatrixView.Rebind();
    }


    protected void ddlvalue_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindData();
        gvmatrixView.Rebind();
    }
    public void BindDataExp()
    {
        try
        {
            DataSet ds = null;
            if (rblFormat.SelectedIndex == 2)
            {
                ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixAllRankExpList(General.GetNullableInteger(ViewState["VESSELTYPE"].ToString())
                        , General.GetNullableInteger(ddlvalue.SelectedValue.ToString()));
                gvmatrixexp.DataSource = ds;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvmatrixexp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataExp();
    }
    protected void gvmatrixexp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlRankselect = (RadComboBox)e.Item.FindControl("ddlRankselect");
            DataSet ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixRankList(0, null);
            ddlRankselect.DataSource = ds;
            ddlRankselect.DataBind();

            RadComboBox ddlRank = (RadComboBox)e.Item.FindControl("ddlRank");
            ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixRankList(0, null);
            ddlRank.DataSource = ds;
            ddlRank.DataBind();

            RadComboBox ddlvesseltypeselect = (RadComboBox)e.Item.FindControl("ddlvesseltypeselect");
            DataSet ds1 = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixVesselTypeList(0, null);
            ddlvesseltypeselect.DataSource = ds1;
            ddlvesseltypeselect.DataBind();



        }
        if (e.Item is GridDataItem)
        {

            RadComboBox ddlRankselectedit = (RadComboBox)e.Item.FindControl("ddlRankselectedit");
            if (ddlRankselectedit != null)
            {
                DataSet ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixRankList(0, null);
                ddlRankselectedit.DataSource = ds;
                ddlRankselectedit.DataBind();

                RadLabel lblranklistedit = (RadLabel)e.Item.FindControl("lblranklistedit");
                SetCsvValue(ddlRankselectedit, lblranklistedit.Text);
            }
            RadComboBox ddlvesseltypeselectedit = (RadComboBox)e.Item.FindControl("ddlvesseltypeselectedit");
            if (ddlvesseltypeselectedit != null)
            {
                DataSet ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixVesselTypeList(0, null);
                ddlvesseltypeselectedit.DataSource = ds;
                ddlvesseltypeselectedit.DataBind();

                RadLabel lbltypelistedit = (RadLabel)e.Item.FindControl("lbltypelistedit");
                SetCsvValue(ddlvesseltypeselectedit, lbltypelistedit.Text);
            }


        }
    }
    protected void gvmatrixexp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "ADD")
        {
            RadComboBox ddlRank = (RadComboBox)e.Item.FindControl("ddlRank");
            RadComboBox ddlvesseltypeselect = (RadComboBox)e.Item.FindControl("ddlvesseltypeselect");
            RadComboBox ddlRankselect = (RadComboBox)e.Item.FindControl("ddlRankselect");

            RadTextBox txtrankexp = (RadTextBox)e.Item.FindControl("txtrankexp");
            RadTextBox txtvesseltypeexp = (RadTextBox)e.Item.FindControl("txtvesseltypeexp");


            DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(General.GetNullableInteger(ViewState["VESSELTYPE"].ToString())
                                                                              , General.GetNullableInteger(ddlRank.SelectedValue.ToString())
                                                                              , General.GetNullableInteger(ddlvalue.SelectedValue.ToString()));

            if (dt.Rows.Count > 0)
            {
                PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixExperienceInsert(int.Parse(dt.Rows[0]["FLDMATRIXID"].ToString())
               , int.Parse(ViewState["VESSELTYPE"].ToString())
               , int.Parse(ddlvalue.SelectedValue.ToString())
               , int.Parse(dt.Rows[0]["FLDRANKID"].ToString())
               , int.Parse(dt.Rows[0]["FLDMANAGEMENTCOMPANYID"].ToString())             
               , General.GetNullableString(GetCsvValue(ddlRankselect))
               , General.GetNullableInteger(txtrankexp.Text)
               , General.GetNullableString(GetCsvValue(ddlvesseltypeselect))
               , General.GetNullableInteger(txtvesseltypeexp.Text)              
               , 0
               , 0
              
               );
                gvmatrixexp.Rebind();
            }
            else
            {
                PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixExperienceInsert(null
               , int.Parse(ViewState["VESSELTYPE"].ToString())
               , int.Parse(ddlvalue.SelectedValue.ToString())
               , int.Parse(ddlRank.SelectedValue)
               , null
               , General.GetNullableString(GetCsvValue(ddlRankselect))
               , General.GetNullableInteger(txtrankexp.Text)
               , General.GetNullableString(GetCsvValue(ddlvesseltypeselect))
               , General.GetNullableInteger(txtvesseltypeexp.Text)
               , 0
               , 0
             
               );
                gvmatrixexp.Rebind();
            }

        }
        if (e.CommandName.ToUpper() == "SAVE")
        {
            Label lblrankidedit = (Label)e.Item.FindControl("lblrankidedit");
            RadComboBox ddlvesseltypeselect = (RadComboBox)e.Item.FindControl("ddlvesseltypeselectedit");
            RadComboBox ddlRankselect = (RadComboBox)e.Item.FindControl("ddlRankselectedit");
            Label ddlRank = (Label)e.Item.FindControl("lblrankidedit");
            RadTextBox txtrankexp = (RadTextBox)e.Item.FindControl("txtrankexpedit");
            RadTextBox txtvesseltypeexp = (RadTextBox)e.Item.FindControl("txtvesseltypeexpedit");
            DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(General.GetNullableInteger(ViewState["VESSELTYPE"].ToString())
                                                                            , General.GetNullableInteger(lblrankidedit.Text)
                                                                            , General.GetNullableInteger(ddlvalue.SelectedValue.ToString()));

            if (dt.Rows.Count > 0)
            {

                PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixExperienceInsert(int.Parse(dt.Rows[0]["FLDMATRIXID"].ToString())
              , int.Parse(ViewState["VESSELTYPE"].ToString())
              , int.Parse(ddlvalue.SelectedValue.ToString())
              , int.Parse(ddlRank.Text)
              , null
              , General.GetNullableString(GetCsvValue(ddlRankselect))
              , General.GetNullableInteger(txtrankexp.Text)
              , General.GetNullableString(GetCsvValue(ddlvesseltypeselect))
              , General.GetNullableInteger(txtvesseltypeexp.Text)
              , 0
              , 0
             
              );
            }
            gvmatrixexp.Rebind();
        }

    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }

}


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

public partial class RegistersFlagDocument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        confirm.Attributes.Add("style", "display:none");

        if (!IsPostBack)
        {
            BindVesselTypeList();
            BindGroupRankList();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersFlagDocument.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbar.Show();

    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            BindData();
            gvmatrixView.Rebind();
        }
    }
    private void BindGroupRankList()
    {
        DataSet ds = PhoenixRegistersGroupRank.ListGroupRank();
        ddlGroupRank.DataSource = ds;
        ddlGroupRank.DataBind();
    }
    private void BindVesselTypeList()
    {
        //DataSet ds = PhoenixRegistersVesselType.ListVesselType(0);
        //chkVesselTypeList.DataSource = ds;
        //chkVesselTypeList.DataTextField = "FLDTYPEDESCRIPTION";
        //chkVesselTypeList.DataValueField = "FLDVESSELTYPEID";
        //chkVesselTypeList.DataBind();
    }
    protected void confirm_Click(object sender, EventArgs e)
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
                string docid = ((Label)e.Item.FindControl("lbldocid")).Text;
                string doctype = ((Label)e.Item.FindControl("lbldoctype")).Text;
                string lblcode = ((Label)e.Item.FindControl("lblcode")).Text;
                DataTable header = ds.Tables[1];
                DataTable dtdocs = ds.Tables[0];
                DataTable dtvalues = ds.Tables[2];

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] drdetails = dtvalues.Select("FLDDOCUMENTID = " + docid +
                                                " AND FLDDOCMENTTYPE = " + doctype +
                                                " AND  FLDFLAGHEADING = '" + header.Rows[i]["FLDHEADINGNAME"].ToString() + "'" +
                                                " AND  FLDOCCODE = '" + lblcode + "'");

                    RadCheckBox chkdetails = new RadCheckBox();
                    Label lblTQMatrix = new Label();
                    Label lblFlagid = new Label();
                    chkdetails.Visible = true;
                    //chkdetails.Enabled = false;
                    if (dtvalues.Rows.Count > 0 && Convert.ToInt16(drdetails[0]["FLDHAVING"].ToString()) > 0)
                        chkdetails.Checked = true;//.Text = drdetails[0]["FLDHAVING"].ToString();                    
                    else
                        chkdetails.Checked = false;

                    chkdetails.AutoPostBack = true;
                    chkdetails.CommandName = "SELECT";
                    chkdetails.CheckedChanged += new EventHandler(ck_CheckedChanged);
                    //chkdetails.Attributes.Add("onchange", "submitit(this);");
                    if (dtvalues.Rows.Count > 0)
                    {

                        lblFlagid.Text = drdetails[0]["FLDFLAGID"].ToString();
                    }


                    //e.Item.Cells[i + 4].Attributes.Add("lblTQMatrix", lblTQMatrix.Text);
                    e.Item.Cells[i + 4].Controls.Add(chkdetails);
                    e.Item.Cells[i + 4].Attributes.Add("lblFlagid", lblFlagid.Text);

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

                string flagid = ((GridTableCell)(((RadCheckBox)e.CommandSource).Parent)).Attributes["lblFlagid"];

                if (((bool)ViewState["CHECKED"]) == true)
                {
                    PhoenixRegistersFlagDocument.FlagDocumentEndorsementUpdate(General.GetNullableInteger(lbldocid), General.GetNullableInteger(ddlvesseltype.SelectedHard)
                        , General.GetNullableInteger(ddlGroupRank.SelectedValue), General.GetNullableInteger(flagid), lblcode);

                }
                if (((bool)ViewState["CHECKED"]) == false)
                {
                    PhoenixRegistersFlagDocument.FlagDocumentEndorsementRemoveUpdate(General.GetNullableInteger(lbldocid), General.GetNullableInteger(ddlvesseltype.SelectedHard)
                           , General.GetNullableInteger(ddlGroupRank.SelectedValue), General.GetNullableInteger(flagid), lblcode);

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
        DataSet ds = null;
        try
        {
            ds = PhoenixRegistersFlagDocument.FlagDocumentSearch(General.GetNullableInteger(ddlvesseltype.SelectedHard)
                    , General.GetNullableInteger(ddlGroupRank.SelectedValue));
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void confirm_Click1(object sender, EventArgs e)
    {

    }


}
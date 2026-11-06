-- View: public.vReceipts

-- DROP VIEW public."vReceipts";

CREATE OR REPLACE VIEW public."vReceipts"
 AS
 SELECT r."Id",
    r."Date",
    r."ApplicationUserId",
    u."Name" AS "UserName",
    u."CompanyId",
    r."CostCenterId",
    cc."Name" AS "CostCenterName",
    r."Number",
    r."Description",
    r."Amount",
    r."OrderId",
    r."Active",
    r."InsertDate",
    r."InsertUser",
    r."UpdateDate",
    r."UpdateUser"
   FROM "Receipts" r
     JOIN "AspNetUsers" u ON u."Id" = r."ApplicationUserId"
     JOIN "CostCenters" cc ON cc."Id" = r."CostCenterId";

ALTER TABLE public."vReceipts"
    OWNER TO postgres;


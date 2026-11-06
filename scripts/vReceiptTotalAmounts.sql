-- View: public.vReceiptTotalAmounts

-- DROP VIEW public."vReceiptTotalAmounts";

CREATE OR REPLACE VIEW public."vReceiptTotalAmounts"
 AS
 SELECT "ApplicationUserId" AS "Id",
    sum("Amount") AS "Total"
   FROM "Receipts"
  WHERE "Active" = true
  GROUP BY "ApplicationUserId";

ALTER TABLE public."vReceiptTotalAmounts"
    OWNER TO postgres;


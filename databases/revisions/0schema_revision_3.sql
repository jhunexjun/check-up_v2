/* Revision date: 10/14/2014 */

/*
SELECT inventorytransfer.docId, inventorytransfer_item.itemCode,
inventorytransfer.createDate inventoryDate,
pricelist.createDate pricelistCreateDate,
pricelisthistory.createDate priceHistoryDate,
pricelist.netPrice pricelistNetPrice,
inventorytransfer_item.realBsNetPrcRtl, pricelisthistory.netPrice pricelisthistoryNetPrice,
ABS(DATEDIFF(inventorytransfer.createDate, pricelisthistory.createDate)) diff
FROM inventorytransfer JOIN inventorytransfer_item USING(docId) JOIN pricelisthistory USING(itemCode)
   JOIN pricelist USING(itemCode) JOIN itemmasterdata USING(itemCode)
   WHERE pricelisthistory.priceListCode = 1 AND pricelisthistory.priceListCode = pricelist.priceListCode
   -- and inventorytransfer_item.itemCode = 'MAINITM151'
   AND ABS(DATEDIFF(inventorytransfer.createDate, pricelisthistory.createDate));
*/

-- this should have been decimal(14, 6). But so far no transactions yet.
ALTER TABLE inventorytransfer MODIFY totalPrcntDscntRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer MODIFY totalAmtDscntRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer MODIFY netTotalRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer MODIFY grossTotalRtl DECIMAL(14,6);

-- same here
ALTER TABLE inventorytransfer_item MODIFY qtyPrRtlUoM DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY realBsNetPrcRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY realBsGrossPrcRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY realNetPrcRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY realGrossPrcRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY netPrcRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY grossPrcRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY prcntDscntRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY amtDscntRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY rowNetTotalRtl DECIMAL(14,6);
ALTER TABLE inventorytransfer_item MODIFY rowGrossTotalRtl DECIMAL(14,6);

UPDATE inventorytransfer_item SET qtyPrRtlUoM = (SELECT qtyPrSaleUoM FROM itemmasterdata WHERE itemmasterdata.itemCode = inventorytransfer_item.itemCode);
UPDATE inventorytransfer_item SET realBsNetPrcRtl = (SELECT netPrice FROM pricelist WHERE priceListCode = 1 AND pricelist.itemCode = inventorytransfer_item.itemCode);
UPDATE inventorytransfer_item SET realBsGrossPrcRtl = realBsNetPrcRtl;
UPDATE inventorytransfer_item SET realNetPrcRtl = qty * qtyPrRtlUoM * realBsNetPrcRtl;
UPDATE inventorytransfer_item SET realGrossPrcRtl = realNetPrcRtl;
UPDATE inventorytransfer_item SET netPrcRtl = realNetPrcRtl;
UPDATE inventorytransfer_item SET grossPrcRtl = netPrcRtl;
UPDATE inventorytransfer_item SET rowNetTotalRtl = netPrcRtl;
UPDATE inventorytransfer_item SET rowGrossTotalRtl = rowNetTotalRtl;

UPDATE inventorytransfer 
INNER JOIN (
  SELECT docId, ROUND(SUM(rowNetTotalRtl), 2) AS rowNetTotal
  FROM inventorytransfer_item
  GROUP BY docId
) inventorytransfer_item ON inventorytransfer.docId = inventorytransfer_item.docId
SET inventorytransfer.netTotalRtl = inventorytransfer_item.rowNetTotal;

UPDATE inventorytransfer 
INNER JOIN (
  SELECT docId, ROUND(SUM(rowGrossTotalRtl), 2) AS rowGrossTotal
  FROM inventorytransfer_item
  GROUP BY docId
) inventorytransfer_item ON inventorytransfer.docId = inventorytransfer_item.docId
SET inventorytransfer.grossTotalRtl = inventorytransfer_item.rowGrossTotal;

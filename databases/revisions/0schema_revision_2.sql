/* Revision date: 10/13/2014 */

-- Check-up wants the retail price in Inventory Transfer instead of purchase price.
ALTER TABLE inventorytransfer ADD COLUMN totalPrcntDscntRtl DECIMAL(10, 6) NOT NULL AFTER grossTotal;
ALTER TABLE inventorytransfer ADD COLUMN totalAmtDscntRtl DECIMAL(10, 6) NOT NULL AFTER totalPrcntDscntRtl;
ALTER TABLE inventorytransfer ADD COLUMN netTotalRtl DECIMAL(10, 6) NOT NULL AFTER totalAmtDscntRtl;
ALTER TABLE inventorytransfer ADD COLUMN grossTotalRtl DECIMAL(10, 6) NOT NULL AFTER netTotalRtl;

ALTER TABLE inventorytransfer_item ADD COLUMN qtyPrRtlUoM DECIMAL(10, 6) NOT NULL AFTER rowGrossTotal;
ALTER TABLE inventorytransfer_item ADD COLUMN realBsNetPrcRtl DECIMAL(10, 6) NOT NULL AFTER qtyPrRtlUoM;
ALTER TABLE inventorytransfer_item ADD COLUMN realBsGrossPrcRtl DECIMAL(10, 6) NOT NULL AFTER realBsNetPrcRtl;
ALTER TABLE inventorytransfer_item ADD COLUMN realNetPrcRtl DECIMAL(10, 6) NOT NULL AFTER realBsGrossPrcRtl;
ALTER TABLE inventorytransfer_item ADD COLUMN realGrossPrcRtl DECIMAL(10, 6) NOT NULL AFTER realNetPrcRtl;
ALTER TABLE inventorytransfer_item ADD COLUMN netPrcRtl DECIMAL(10, 6) NOT NULL AFTER realGrossPrcRtl;
ALTER TABLE inventorytransfer_item ADD COLUMN grossPrcRtl DECIMAL(10, 6) NOT NULL AFTER netPrcRtl;
ALTER TABLE inventorytransfer_item ADD COLUMN prcntDscntRtl DECIMAL(10, 6) NOT NULL AFTER grossPrcRtl;
ALTER TABLE inventorytransfer_item ADD COLUMN amtDscntRtl DECIMAL(10, 6) NOT NULL AFTER prcntDscntRtl;
ALTER TABLE inventorytransfer_item ADD COLUMN rowNetTotalRtl DECIMAL(10, 6) NOT NULL AFTER amtDscntRtl;
ALTER TABLE inventorytransfer_item ADD COLUMN rowGrossTotalRtl DECIMAL(10, 6) NOT NULL AFTER rowNetTotalRtl;

-- this should be not null
ALTER TABLE warehouse MODIFY `code` VARCHAR(16) NOT NULL;
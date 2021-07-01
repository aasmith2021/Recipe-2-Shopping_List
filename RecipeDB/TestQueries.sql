BEGIN TRANSACTION;
SELECT *
FROM instruction_block ib
JOIN instruction_block_instruction ibi ON ib.ib_id = ibi.instruction_block_id
JOIN instruction i ON ibi.instruction_id = i.inst_id;

/*
DELETE FROM instruction_block_instruction WHERE instruction_block_id = 1;
DELETE FROM instruction WHERE inst_id IN (SELECT i.inst_id FROM instruction i JOIN instruction_block_instruction ibi ON i.inst_id = ibi.instruction_id JOIN instruction_block ib ON ibi.instruction_block_id = ib.ib_id WHERE ib.ib_id = 1);

DELETE FROM instruction_block_instruction WHERE instruction_block_id = 1;
DELETE FROM instruction WHERE inst_id IN (SELECT i.inst_id FROM instruction i JOIN instruction_block_instruction ibi ON i.inst_id = ibi.instruction_id JOIN instruction_block ib ON ibi.instruction_block_id = ib.ib_id WHERE ib.ib_id = 1);
*/

SELECT *
FROM instruction_block ib
JOIN instruction_block_instruction ibi ON ib.ib_id = ibi.instruction_block_id
JOIN instruction i ON ibi.instruction_id = i.inst_id;
ROLLBACK;
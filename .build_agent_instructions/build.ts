#!/usr/bin/env npx tsx
/**
 * Builds proprietary agent instruction files from a set of source-of-truth docs.
 *
 * This script reads a YAML configuration file, loads a set of source documents,
 * and renders them into a target-specific format using a Nunjucks template.
 * This decouples the core documentation from proprietary agent instruction formats.
 *
 * Usage:
 *   tsx build.ts --target github
 *   tsx build.ts # Defaults to building all targets
 */

import * as yaml from "js-yaml";
import * as fs from "node:fs";
import * as path from "node:path";
import { fileURLToPath } from "node:url";
import nunjucks from "nunjucks";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const REPO_ROOT = path.resolve(__dirname, "..");
const CONFIG_PATH = path.join(REPO_ROOT, ".build_agent_instructions", "config.yml");

interface SourceInfo {
  id: string;
  path: string;
}

interface TargetInfo {
  template: string;
  output: string;
}

interface Config {
  sources: SourceInfo[];
  targets: Record<string, TargetInfo>;
}

interface SourceDoc {
  id: string;
  path: string;
  content: string;
}

interface ParsedArgs {
  target?: string;
}

const isFileNotFoundError = (error: unknown): boolean =>
  (error as NodeJS.ErrnoException).code === "ENOENT";

const loadConfig = (): Config => {
  try {
    const content = fs.readFileSync(CONFIG_PATH, "utf-8");
    return yaml.load(content) as Config;
  } catch (error) {
    if (isFileNotFoundError(error)) {
      console.error(`ERROR: Config file not found at ${CONFIG_PATH}`);
    } else if (error instanceof yaml.YAMLException) {
      console.error(`ERROR: Could not parse YAML config at ${CONFIG_PATH}: ${error.message}`);
    }
    throw error;
  }
};

const loadSources = (config: Config): Record<string, SourceDoc> => {
  const sources: Record<string, SourceDoc> = {};

  for (const sourceInfo of config.sources ?? []) {
    const docId = sourceInfo.id;
    const filePath = path.join(REPO_ROOT, sourceInfo.path);

    try {
      const content = fs.readFileSync(filePath, "utf-8").trim();
      sources[docId] = { id: docId, path: filePath, content };
    } catch (error) {
      if (isFileNotFoundError(error)) {
        console.warn(`WARNING: Source file not found: ${filePath}`);
        sources[docId] = {
          id: docId,
          path: filePath,
          content: `File not found: ${filePath}`,
        };
      } else {
        throw error;
      }
    }
  }

  return sources;
};

const buildTarget = (
  targetName: string,
  config: Config,
  sources: Record<string, SourceDoc>
): void => {
  const targetInfo = config.targets?.[targetName];
  if (!targetInfo) {
    console.error(`ERROR: Target '${targetName}' not found in config.`);
    return;
  }

  const templatePath = path.join(REPO_ROOT, targetInfo.template);
  const outputPath = path.join(REPO_ROOT, targetInfo.output);

  console.log(`Building target '${targetName}':`);
  console.log(`  Template: ${path.relative(REPO_ROOT, templatePath)}`);
  console.log(`  Output:   ${path.relative(REPO_ROOT, outputPath)}`);

  let templateContent: string;
  try {
    templateContent = fs.readFileSync(templatePath, "utf-8");
  } catch (error) {
    if (isFileNotFoundError(error)) {
      console.error(`ERROR: Template not found: ${templatePath}`);
      return;
    }
    throw error;
  }

  // Nunjucks configured to match Jinja2 behavior from Python version
  const env = nunjucks.configure(REPO_ROOT, {
    trimBlocks: true,
    lstripBlocks: true,
    autoescape: false,
  });

  const renderedContent = env.renderString(templateContent, { sources }).trimEnd();

  fs.mkdirSync(path.dirname(outputPath), { recursive: true });
  fs.writeFileSync(outputPath, renderedContent, "utf-8");
  console.log(`Successfully wrote ${path.relative(REPO_ROOT, outputPath)}`);
};

const parseArgs = (): ParsedArgs => {
  const args = process.argv.slice(2);
  const result: ParsedArgs = {};

  for (let i = 0; i < args.length; i++) {
    if (args[i] === "--target" && args[i + 1]) {
      result.target = args[i + 1];
      i++;
    }
  }

  return result;
};

const main = (): number => {
  const args = parseArgs();
  const config = loadConfig();
  const sources = loadSources(config);

  if (args.target) {
    if (!config.targets?.[args.target]) {
      console.error(`ERROR: Target '${args.target}' not defined in ${CONFIG_PATH}`);
      return 1;
    }
    buildTarget(args.target, config, sources);
  } else {
    console.log("Building all targets...");
    for (const targetName of Object.keys(config.targets ?? {})) {
      buildTarget(targetName, config, sources);
      console.log("-".repeat(20));
    }
  }

  return 0;
};

process.exit(main());
